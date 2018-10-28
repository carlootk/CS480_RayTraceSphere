using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace CS480_RayTraceSphere
{

    /// <summary>
    /// Sphere struct for sphering.
    /// </summary>
    public struct Sphere
    {
        public int x, y, z, r;

        public Sphere(int x, int y, int z, int r)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.r = r;
        }
    }

    public partial class Form1 : Form
    {
        Vector3 light, user;
        Sphere sphere;
        public Form1()
        {
            InitializeComponent();
            user = new Vector3(400, 400, -400);
            light = new Vector3(300, 10, 10);
            sphere = new Sphere(200, 400, 100, 60);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Create bitmap to draw sphere
            Bitmap bitmap = new Bitmap(canvas.ClientSize.Width, canvas.ClientSize.Height);
            //Followed formulas to create different values from formulas.
            using (Graphics graphic = Graphics.FromImage(bitmap))
            {
                //Loop through x on canvas.
                for (int xPix = 0; xPix < canvas.ClientSize.Width; xPix++)
                {
                    //Loop through y on canvas.
                    for (int yPix = 0; yPix < canvas.ClientSize.Height; yPix++)
                    {
                        //calculate values for formula => at^2 - 2tb + c
                        float a = (user.X - xPix) * (user.X - xPix) + (user.Y - yPix) * (user.Y - yPix) + user.Z * user.Z;
                        float b = 2 * ((user.X - xPix) * (user.X - sphere.x) + (user.Y - yPix) * (user.Y - sphere.y) + user.Z * (user.Z - sphere.z));
                        float c = sphere.x * sphere.x + sphere.y * sphere.y + sphere.z * sphere.z + user.X * user.X + user.Y * user.Y + user.Z * user.Z - 2 * (sphere.x * user.X + sphere.y * user.Y + sphere.z * user.Z) - sphere.r * sphere.r;
                        //discriminant
                        float d = b * b - 4 * a * c;
                        //initialize t to miss
                        float t = -1;
                        //Find the closest t value, if not t will be negative which is non-hit.
                        //Debug.Print(d.ToString());
                        if (d >= 0)
                        {
                            float t1 = (-b - (float)Math.Sqrt(d)) / (2.0f * a);
                            float t2 = (-b + (float)Math.Sqrt(d)) / (2.0f * a);
                            if (t1 > t2)
                                t = t1;
                            else
                                t = t2;
                            //calculate point of intersection
                            float x = user.X + t * (user.X - xPix);
                            float y = user.Y + t * (user.Y - yPix);
                            float z = user.Z + t * user.Z;
                            //find normal vector and unitvector
                            Vector3 normal = new Vector3(((x - sphere.x) / (sphere.r)), ((y - sphere.y) / sphere.r), ((z - sphere.z) / sphere.r));
                            Vector3 lPrime = new Vector3(light.X - x, light.Y - y, light.Z - z);
                            Vector3 unitVector = lPrime / ((float)Math.Sqrt(lPrime.X * lPrime.X + lPrime.Y * lPrime.Y + (lPrime.Z * lPrime.Z)));
                            //find angle between normal and unit vector
                            float cosTheta = ((normal.X * unitVector.X + normal.Y * unitVector.Y + normal.Z * unitVector.Z) /
                                ((float)Math.Sqrt((unitVector.X * unitVector.X + unitVector.Y * unitVector.Y + unitVector.Z * unitVector.Z)) *
                                (float)Math.Sqrt((normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z))));
                            //if costheta is less than 0, ambient light
                            if (cosTheta < .0000000001)
                            {
                                bitmap.SetPixel(xPix, yPix, Color.FromArgb(55, 0, 0));
                            }
                            //else costheta times 200 + ambient light
                            else
                            {
                                bitmap.SetPixel(xPix, yPix, Color.FromArgb((int)(cosTheta * 200 + 55), 0, 0));
                            }

                        }
                    }
                }
                //set image to bitmap
                canvas.Image = bitmap;
            }
        }
    }
}
