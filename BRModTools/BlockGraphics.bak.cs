using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Tao.FreeGlut;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Platform;


namespace BRModTools
{
    public partial class BlockInfo : Form
    {
      

         private bool loaded = false;
 
        // initialisation
        private void init()
        {
            // initialise Glut
            Glut.glutInit();

            GL.Enable(EnableCap.DepthTest); // Está desactivado por defecto en OpenGL
            GL.Enable(EnableCap.CullFace); // Está desactivado por defecto en OpenGL
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);

            
 
            float[] mat_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] mat_shininess = { 50.0f };
            float[] light_position = { 1.0f, 1.0f, 1.0f, 0.0f };
            float[] light_ambient = { 0.5f, 0.5f, 0.5f, 1.0f };
 
            GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.ShadeModel(ShadingModel.Smooth);

            GL.Material(MaterialFace.Front, MaterialParameter.Specular, mat_specular);
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, mat_shininess);
            GL.Light(LightName.Light0, LightParameter.Position, light_position);
            GL.Light(LightName.Light0, LightParameter.Ambient, light_ambient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, mat_specular);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.CullFace);

        }
 
        // draws the scene
        private void display()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
 
            GL.PushMatrix();
            {
                GL.Rotate(10, 1, 0, 0);
                GL.Rotate(10, 0, 1, 0);
 
                GL.Color3(0.0, 0.5, 1.0);
 
 
                // *** SET THIS TO FALSE TO CHANGE TO GLUTSOLIDCUBE ***
                bool drawQuads = true;
 
                if (drawQuads)
                {
                    GL.Begin(BeginMode.Quads);
                    {

                        GL.Color3(Color.Silver);
                        GL.Vertex3(-1.0f, -1.0f, -1.0f);
                        GL.Vertex3(-1.0f, 1.0f, -1.0f);
                        GL.Vertex3(1.0f, 1.0f, -1.0f);
                        GL.Vertex3(1.0f, -1.0f, -1.0f);

                        GL.Color3(Color.Honeydew);
                        GL.Vertex3(-1.0f, -1.0f, -1.0f);
                        GL.Vertex3(1.0f, -1.0f, -1.0f);
                        GL.Vertex3(1.0f, -1.0f, 1.0f);
                        GL.Vertex3(-1.0f, -1.0f, 1.0f);

                        GL.Color3(Color.Moccasin);

                        GL.Vertex3(-1.0f, -1.0f, -1.0f);
                        GL.Vertex3(-1.0f, -1.0f, 1.0f);
                        GL.Vertex3(-1.0f, 1.0f, 1.0f);
                        GL.Vertex3(-1.0f, 1.0f, -1.0f);

                        GL.Color3(Color.IndianRed);
                        GL.Vertex3(-1.0f, -1.0f, 1.0f);
                        GL.Vertex3(1.0f, -1.0f, 1.0f);
                        GL.Vertex3(1.0f, 1.0f, 1.0f);
                        GL.Vertex3(-1.0f, 1.0f, 1.0f);

                        GL.Color3(Color.PaleVioletRed);
                        GL.Vertex3(-1.0f, 1.0f, -1.0f);
                        GL.Vertex3(-1.0f, 1.0f, 1.0f);
                        GL.Vertex3(1.0f, 1.0f, 1.0f);
                        GL.Vertex3(1.0f, 1.0f, -1.0f);

                        GL.Color3(Color.ForestGreen);
                        GL.Vertex3(1.0f, -1.0f, -1.0f);
                        GL.Vertex3(1.0f, 1.0f, -1.0f);
                        GL.Vertex3(1.0f, 1.0f, 1.0f);
                        GL.Vertex3(1.0f, -1.0f, 1.0f);
                       /* // front face
                        GL.Color3(Color.Silver);
                        GL.Normal3(0.0, 0.0, 1.0);
                        GL.Vertex3(-0.5, -0.5, 0.5);
                        GL.Vertex3(0.5, -0.5, 0.5);
                        GL.Vertex3(0.5, 0.5, 0.5);
                        GL.Vertex3(-0.5, 0.5, 0.5);
 
                        // back face
                        GL.Color3(Color.Honeydew);
                        GL.Normal3(0.0, 0.0, -1.0);
                        GL.Vertex3(-0.5, -0.5, -0.5);
                        GL.Vertex3(-0.5, 0.5, -0.5);
                        GL.Vertex3(0.5, 0.5, -0.5);
                        GL.Vertex3(0.5, -0.5, -0.5);
 
                        // top face
                        GL.Color3(Color.Moccasin);
                        GL.Normal3(0.0, 1.0, 0.0);
                        GL.Vertex3(-0.5, 0.5, -0.5);
                        GL.Vertex3(-0.5, 0.5, 0.5);
                        GL.Vertex3(0.5, 0.5, 0.5);
                        GL.Vertex3(0.5, 0.5, -0.5);
 
                        // bottom face
                        GL.Color3(Color.IndianRed);
                        GL.Normal3(0.0, -1.0, 0.0);
                        GL.Vertex3(-0.5, -0.5, -0.5);
                        GL.Vertex3(0.5, -0.5, -0.5);
                        GL.Vertex3(0.5, -0.5, 0.5);
                        GL.Vertex3(-0.5, -0.5, 0.5);
 
                        // right face
                        GL.Color3(Color.PaleVioletRed);
                        GL.Normal3(1.0, 0.0, 0.0);
                        GL.Vertex3(0.5, -0.5, -0.5);
                        GL.Vertex3(0.5, 0.5, -0.5);
                        GL.Vertex3(0.5, 0.5, 0.5);
                        GL.Vertex3(0.5, -0.5, 0.5);
 
                        // left face
                        GL.Color3(Color.ForestGreen);
                        GL.Normal3(-1.0, 0.0, 0.0);
                        GL.Vertex3(-0.5, -0.5, -0.5);
                        GL.Vertex3(-0.5, -0.5, 0.5);
                        GL.Vertex3(-0.5, 0.5, 0.5);
                        GL.Vertex3(-0.5, 0.5, -0.5);*/
                    }
                    GL.End();
                }
                else
                {
                    Glut.glutSolidCube(1.0);
                }
            }
            GL.PopMatrix();
 
            GL.Flush();
        }
 
        // resizes the viewport
        private void reshape(int w, int h)
        {
            GL.Viewport(0, 0, w, h);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            if (w <= h)
            {
                GL.Ortho(-1.5, 1.5, -1.5 * (double)h / (double)w, 1.5 * (double)h / (double)w, -10.0, 10.0);
            }
            else
            {
                GL.Ortho(-1.5 * (double)w / (double)h, 1.5 * (double)w / (double)h, -1.5, 1.5, -10.0, 10.0);
            }
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }
 
        // GLControl resize event handler
        private void glControl1_Resize(object sender, EventArgs e)
        {
            if (!loaded) return;
            reshape(glControl1.Width, glControl1.Height);
        }
 
        // GLControl paint event handler
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded) return;
            display();
            glControl1.SwapBuffers();
        }
 
        // Main form load event handler
        private void Form1_Load(object sender, EventArgs e)
        {
            loaded = true;
            init();
            reshape(glControl1.Width, glControl1.Height);
        }
    }
}