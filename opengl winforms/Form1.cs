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
        public Form1()
        {
            InitializeComponent();

            //assign event handlers to the proper functions
            this.glControl1.ContextCreated += new System.EventHandler<OpenGL.GlControlEventArgs>(this.Render_CreateContext);
            this.glControl1.ContextDestroying += new System.EventHandler<OpenGL.GlControlEventArgs>(this.Render_DestroyContext);
            this.glControl1.Render += new System.EventHandler<OpenGL.GlControlEventArgs>(this.Render_UpdateContext);
            this.glControl1.ContextUpdate += new System.EventHandler<OpenGL.GlControlEventArgs>(this.Render_OnUpdate);
        }
               
        private void Render_CreateContext(object sender, GlControlEventArgs e)
        {
            //code executed when render context is created

            GlControl glControl = (GlControl)sender;    //get control that triggered this
            
        }

        private void Render_DestroyContext(object sender, GlControlEventArgs e)
        {
            //code executed when render context is destroyed
        }

        private void Render_OnUpdate(object sender, GlControlEventArgs e)
        {
            //code executed as the logic for each update i think?
        }

        private void Render_UpdateContext(object sender, GlControlEventArgs e)
        {
            //code executed to render the update iguess
            //not sure how this is different to onupdate
        }



        //data to actually do stuf

        private static float _Angle;

        /// <summary>
        /// Vertex position array.
        /// </summary>
        private static readonly float[] _ArrayPosition = new float[] {
            0.0f, 0.0f,
            1.0f, 0.0f,
            1.0f, 1.0f
        };

        /// <summary>
        /// Vertex color array.
        /// </summary>
        private static readonly float[] _ArrayColor = new float[] {
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
