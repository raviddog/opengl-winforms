using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Khronos;
using OpenGL;


namespace opengl_winforms
{
    public partial class Form1 : Form
    {
        //our shader and VAO
        private ShaderProgram shaderProgram;
        private GLVertexArrayObject vao;

        //
        private static float angle; //its static so it starts at 0.00f. we'll use this to rotate our triangle

        public Form1()
        {
            InitializeComponent();

            //assign event handlers to the proper functions
            this.glControl1.ContextCreated += new System.EventHandler<OpenGL.GlControlEventArgs>(this.Render_CreateContext);
            this.glControl1.ContextDestroying += new System.EventHandler<OpenGL.GlControlEventArgs>(this.Render_DestroyContext);
            this.glControl1.Render += new System.EventHandler<OpenGL.GlControlEventArgs>(this.Render_Render);
            this.glControl1.ContextUpdate += new System.EventHandler<OpenGL.GlControlEventArgs>(this.Render_ContextUpdate);
        }
               
        private void Render_CreateContext(object sender, GlControlEventArgs e)
        {
            //code executed when render context is created

            //create the shader, this compiles it and all that good stuff
            shaderProgram = new ShaderProgram(_VertexShader, _FragmentShader);
            vao = new GLVertexArrayObject(shaderProgram, _PositionData, _ColorData);
        }

        private void Render_DestroyContext(object sender, GlControlEventArgs e)
        {
            //code executed when render context is destroyed
            shaderProgram?.Dispose();
            vao?.Dispose();
        }

        private void Render_ContextUpdate(object sender, GlControlEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("update angle");
            //code executed as the logic for each update i think?
            angle += 1.0f;
            if(angle > 360.0f)
            {
                //angle goes from 0.0f to 360.0f
                angle -= 360.0f;
            }
        }

        private void Render_Render(object sender, GlControlEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("render");
            //code executed to render the update iguess
            //not sure how this is different to onupdate

            //get a reference to the object that contains our opengl crap
            Control senderControl = (Control)sender;
            //set the size of our OpenGL rendering area. we're startign in the very corner and
            //getting the height and the width of the blank canvas control in our form
            Gl.Viewport(0, 0, senderControl.Size.Width, senderControl.Size.Height);
            //fill the viewport with a color
            //basically wipe it clean for a new rendering frame
            Gl.Clear(ClearBufferMask.ColorBufferBit);

            //its a matrix. but you dont need to know how matrixes work.
            //the gist of it is that when you multiply a bunch of stuff together you can make 3D
            //opengl does all that hard stuff, you just need to know how to use the function calls

            //the projection matrix turns the 3D space into what your camera sees
            //this particular function tells OpenGL to turn its 3D world into a flat 2D image
            //  that has these dimensions:
            //      100%
            //  100%    100%
            //      100%
            //aka it takes up all of our rendering window.
            Matrix4x4f projection = Matrix4x4f.Ortho2D(-1.0f, 1.0f, -1.0f, 1.0f);

            //the translated matrix moves the model around in 3D space
            //so we'll take our triangle model and move it 
            //to apply multiple matrices to something, you just multiple them together
            //so we're multiplying that by a matrix that rotates the model on the Z axis
            Matrix4x4f model = Matrix4x4f.Translated(-0.5f, -0.5f, 0.0f) * Matrix4x4f.RotatedZ(angle);

            //tell OpenGL to use this shader
            Gl.UseProgram(shaderProgram.ProgramID);
            //send a matrix to our shader program. it will send it to the variable in the shader that we located and
            //  saved into attribUniformLoc
            //a uniform is just a variable in the shader that we manually send data to, instead of the shader
            //  reading that data from a buffer in the gpu vram
            //the matrix we're sending is our projection matrix multiplied by our model matrix
            //the model matrix puts the triangle in the right spot in our 3D world, the projection matrix
            //  makes it draw to the camera properly
            Gl.UniformMatrix4f(shaderProgram.attribUniformLoc, 1, false, projection * model);
            //tell OpenGL to use the state data saved in this Vertex Array Object
            Gl.BindVertexArray(vao.VAO_ID);
            //draw the vertexes
            //it takes the data from the activated buffers and processes it according to how we've told it to interpret it
            //we want to draw triangles, so it will connect every set of 3 vertex data points
            //we're starting from position 0 in the buffers, and processing 3 vertexes
            Gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
            //unbind the vao. its good practice to do this, so you dont accidentally change your vao's state
            Gl.BindVertexArray(0);
            
        }

        /*
         * its 3AM and i dont feel like relearning how
         * Vertex Aray Objects and Vertex Buffer Objects and crap work
         * so im just gonna copy their code
         * its probably for the better anyway
         */

        private class GLBuffer : IDisposable
        {
            //store ID for the buffer
            public readonly uint BufferID;

            public GLBuffer(float[] data)
            {
                if(data != null)
                {
                    //tell the gpu to find a spot for a buffer in vram for us, and gets the ID number for it
                    BufferID = Gl.GenBuffer();
                    //create the buffer
                    Gl.BindBuffer(BufferTarget.ArrayBuffer, BufferID);
                    //send the data to the buffer. the data is (4 bytes per float, times the number of floats in the array) bytes long, and we're using it as unchanging data that we will draw with
                    Gl.BufferData(BufferTarget.ArrayBuffer, (uint)(4 * data.Length), data, BufferUsage.StaticDraw);
                }
            }

            public void Dispose()
            {
                //tell the gpu to delete the buffer when GC deletes this object
                //otherwise we will lose the reference to that buffer, and its data will sit there forever
                //and we wont be able to clear it because we dont know its ID to tell the gpu to delete it
                //aka a memory leak
                Gl.DeleteBuffers(BufferID);
            }
        }


        /*
         * OpenGL is basically a giant state machine, with a ton of variables inside it telling it
         * how to render the data you've given it
         * A Vertex Array Object (VAO) is like a save state for this state machine
         * It lets us set a bunch of state data, and then apply them all at once for like the different drawing
         * methods and stuff we want to do, so eg. a player model, and then the environment, and then the UI, etc
         * 
         * I THINK normally a VAO would be used for a lot more than 1 triangle
         * so having our VAO object be responsible for creating the buffers and stuff is probably not industry standard
         * but its an example so it'll do for now
         */
        private class GLVertexArrayObject : IDisposable
        {
            //ID to this object, a buffer object for position data and a buffer object for color data
            public readonly uint VAO_ID;
            private readonly GLBuffer positionBuffer;
            private readonly GLBuffer colorBuffer;

            public GLVertexArrayObject(ShaderProgram shader, float[] positionData, float[] colorData)
            {
                if(shader != null) //duh?
                {
                    //create the buffer objects for position and color data
                    positionBuffer = new GLBuffer(positionData);
                    colorBuffer = new GLBuffer(colorData);
                    //get an ID for a VAO in the gpu vram. it's not created yet
                    VAO_ID = Gl.GenVertexArray();
                    //now create the VAO and tell OpenGL we want to use it
                    Gl.BindVertexArray(VAO_ID);

                    //tell OpenGL that we are now currently working on the position buffer
                    Gl.BindBuffer(BufferTarget.ArrayBuffer, positionBuffer.BufferID);
                    //tell OpenGL how to interpret the data that is contained inside this buffer
                    //here's what each argument of the function does. this is a very important function.

                    //this buffer contains the data that should be passed to the variable in the shader
                    //  that is at the location specified by attribPositionLoc, aka the "aPosition" variable
                    //there are 2 pieces of data that this variable uses per vertex
                    //the data is of type float
                    //the data describes a position in the virtual 3D space, as opposed to the 2D space on your screen
                    //  (aka OpenGL has to calculate where the 3D point will end up on the 2D screen.
                    //      its the whole 3D depth calculation thingy idk how to explain)
                    //the size of the gap between data for one vertex and data for the next vertex is 0
                    //start processing the data 0 bytes past the location specified
                    Gl.VertexAttribPointer((uint)shader.attribPositionLoc, 2, VertexAttribType.Float, false, 0, IntPtr.Zero);

                    //turn on this attribute
                    Gl.EnableVertexAttribArray((uint)shader.attribPositionLoc);

                    //same as above but for the color buffer
                    Gl.BindBuffer(BufferTarget.ArrayBuffer, colorBuffer.BufferID);
                    Gl.VertexAttribPointer((uint)shader.attribColorLoc, 3, VertexAttribType.Float, false, 0, IntPtr.Zero);
                    Gl.EnableVertexAttribArray((uint)shader.attribColorLoc);
                    //tell OpenGL we want to use the VAO at location 0
                    //there's no VAO there
                    //this basically has the effect of unbinding the VAO, so that we keep its saved state data and dont
                    //  accidentally mess with it further
                    Gl.BindVertexArray(0);
                }
            }

            public void Dispose()
            {
                positionBuffer.Dispose();
                colorBuffer.Dispose();
            }
        }
        
        






        

        //individual shader program class
        //its an IDisposable because there's unmanaged stuff that needs to be deleted when the GC
        //  cleans up this object
        private class ShaderObject : IDisposable
        {
            public readonly uint ShaderID;

            public ShaderObject(ShaderType shaderType, string[] source)
            {
                if(source != null)
                {
                    //if its null you want to throw an exception

                    //tell the gpu to create a shader, and give us the ID number for it
                    ShaderID = Gl.CreateShader(shaderType);
                    //add shader source code to the newly created shader usings its ID
                    Gl.ShaderSource(ShaderID, source);
                    //compile the shader
                    Gl.CompileShader(ShaderID);

                    //check for success
                    Gl.GetShader(ShaderID, ShaderParameterName.CompileStatus, out int compiled);
                    if(compiled == 0)
                    {
                        //it fucked up, throw an error somewhere with like logs and shit
                    }
                }
            }

            public void Dispose()
            {
                //delete the shader when GC cleans up this object
                Gl.DeleteShader(ShaderID);
            }
        }


        //full shader program class
        //its an IDisposable because there's unmanaged stuff that needs to be deleted when the GC
        //  cleans up this object
        private class ShaderProgram : IDisposable
        {
            public readonly uint ProgramID;
            public readonly int attribUniformLoc;
            public readonly int attribPositionLoc;
            public readonly int attribColorLoc;

            public ShaderProgram(string[] vertexSource, string[] fragmentSource)
            {
                //create vertex and fragment shaders
                //create the objects in a way that will call the Dispose function when GC cleans them up
                using (ShaderObject vertex = new ShaderObject(ShaderType.VertexShader, vertexSource))
                using (ShaderObject fragment = new ShaderObject(ShaderType.FragmentShader, fragmentSource))
                {
                    //tell the gpu to create a shader program, and give us the ID number for it
                    ProgramID = Gl.CreateProgram();
                    //connect the vertex and fragment shaders to it
                    Gl.AttachShader(ProgramID, vertex.ShaderID);
                    Gl.AttachShader(ProgramID, fragment.ShaderID);
                    //link program, not sure what this means
                    Gl.LinkProgram(ProgramID);

                    //check for errors
                    Gl.GetProgram(ProgramID, ProgramProperty.LinkStatus, out int linked);
                    if(linked == 0)
                    {
                        //it fucked up
                    }

                    //get the location of the uniform variables in the shaders
                    attribUniformLoc = Gl.GetUniformLocation(ProgramID, "uMVP");
                    //get the location of the position and color attributes in the shaders
                    attribPositionLoc = Gl.GetAttribLocation(ProgramID, "aPosition");
                    attribColorLoc = Gl.GetAttribLocation(ProgramID, "aColor");
                    //you should probably check to make sure those worked too, but im lazy
                }

            }

            public void Dispose()
            {
                Gl.DeleteProgram(ProgramID);   
            }
        }


        

        /// <summary>
        /// Vertex position array.
        /// </summary>
        private static readonly float[] _PositionData = new float[] {
            0.0f, 0.0f,
            1.0f, 0.0f,
            1.0f, 1.0f
        };

        /// <summary>
        /// Vertex color array.
        /// </summary>
        private static readonly float[] _ColorData = new float[] {
            1.0f, 0.0f, 0.0f,
            0.0f, 1.0f, 0.0f,
            0.0f, 0.0f, 1.0f
        };

        //shaders

        private readonly string[] _VertexShader = {
            "#version 150 compatibility\n",
            "uniform mat4 uMVP;\n",
            "in vec2 aPosition;\n",
            "in vec3 aColor;\n",
            "out vec3 vColor;\n",
            "void main() {\n",
            "	gl_Position = uMVP * vec4(aPosition, 0.0, 1.0);\n",
            "	vColor = aColor;\n",
            "}\n"
        };

        private readonly string[] _FragmentShader = {
            "#version 150 compatibility\n",
            "in vec3 vColor;\n",
            "void main() {\n",
            "	gl_FragColor = vec4(vColor, 1.0);\n",
            "}\n"
        };

    }
}
