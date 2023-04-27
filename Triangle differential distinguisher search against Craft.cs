using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//craft
using System;
using Gurobi;
class mip1_cs
{
    static void Main()
    {
        try
        {
            GRBEnv env = new GRBEnv("mip1.log"); GRBModel model = new GRBModel(env);
            // Create variables
            int round =4,round0=4, varnum = 128, i, j, k;//57
            int Bs = 5; int Bl = 4;
            GRBLinExpr Objection = new GRBLinExpr();
            GRBLinExpr[,] obj = new GRBLinExpr[round, 10];
            GRBLinExpr[] Dobj = new GRBLinExpr[round0 + 1];
            GRBLinExpr Pro = new GRBLinExpr();
            for (i = 0; i < round; i++)
                for (j = 0; j < 10; j++)
                    obj[i, j] = new GRBLinExpr();

            for (j = 0; j < round0 + 1; j++)
                Dobj[j] = new GRBLinExpr();

            GRBLinExpr ActiveS = new GRBLinExpr();

            GRBVar[,] P = new GRBVar[round + 1, 16];
            for (i = 0; i < round + 1; i++)
                for (j = 0; j < 16; j++)
                {
                    P[i, j] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, string.Format("P{0}{0}", i, j));
                }
            GRBVar[,] P2 = new GRBVar[round0 + 1, 16]; 
            for (i = 0; i < round0 + 1; i++)
                for (j = 0; j < 16; j++)
                {
                    P2[i, j] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, string.Format("P{0}{0}", i, j));
                }

            GRBVar[,] P3 = new GRBVar[round0 + 1, 16];
            for (i = 0; i < round0 + 1; i++)
                for (j = 0; j < 16; j++)
                    P3[i, j] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, string.Format("P{0}{0}", i, j));

            GRBVar[] P4 = new GRBVar[16];
            for (j = 0; j < 16; j++)
                P4[j] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, string.Format("P{0}{0}", j));


            // GRBVar x = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "x"); GRBVar y = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "y"); GRBVar z = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, "z");
            GRBVar[,] A = new GRBVar[round + 1, 16]; 
            for (i = 0; i < round + 1; i++)
                for (j = 0; j < 16; j++)
                {
                    A[i, j] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, string.Format("A{0}{0}", i, j));
                }

            GRBVar[,] B = new GRBVar[round0 + 1, 16]; 
            for (i = 0; i < round0 + 1; i++)
                for (j = 0; j < 16; j++)
                {
                    B[i, j] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, string.Format("B{0}{0}", i, j));
                }

            GRBVar[,] D = new GRBVar[round0 + 1, 16];
            for (i = 0; i < round0 + 1; i++)
                for (j = 0; j < 16; j++)
                    D[i, j] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, string.Format("A{0}{0}", i, j));

            GRBVar[,] Asc = new GRBVar[round + 1, 16]; 
            for (i = 0; i < round + 1; i++)
                for (j = 0; j < 16; j++)
                {
                    Asc[i, j] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, string.Format("A{0}{0}", i, j));
                }

            GRBVar[,] Bmc = new GRBVar[round0 + 1, 16]; 
            for (i = 0; i < round0 + 1; i++)
                for (j = 0; j < 16; j++)
                {
                    Bmc[i, j] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, string.Format("B{0}{0}", i, j));
                }

            GRBVar[,] Dmc = new GRBVar[round0 + 1, 16];
            for (i = 0; i < round0 + 1; i++)
                for (j = 0; j < 16; j++)
                    Dmc[i, j] = model.AddVar(0.0, 1.0, 0.0, GRB.BINARY, string.Format("B{0}{0}", i, j));

            // model.AddGenConstrMax(max,new GRBVar[]{},0,"c"}
            //设定输入至少一个差分比特：
            for (i = 0; i < 16; i++)
                obj[0, 0] = A[0, i] + obj[0, 0];

            //he!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            model.AddConstr(obj[0, 0] >= 1, "input");
            int[] SC = { 15, 10, 9, 4, 3, 6, 5, 8, 7, 2, 1, 12, 11, 14, 13, 0 };
           

            //解密过程
            for (i = 0; i < round; i++)
            {//p1:4 p0:8 
                for (j = 0; j < 16; j++)
                    model.AddConstr(A[i,j ] == Asc[i, SC[j]], "");
                for (j = 0; j < 4; j++)
                {
                    model.AddConstr(Asc[i, 4 * j + 3] + 1 - A[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - Asc[i, 4 * j + 3] + A[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - A[i + 1, 4 * j] + 1 - A[i + 1, 4 * j + 1] + 1 - P[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(Asc[i, 4 * j + 2] + 1 - A[i + 1, 4 * j + 2] >= 1, "dots");
                    model.AddConstr(1 - Asc[i, 4 * j + 2] + A[i + 1, 4 * j + 2] >= 1, "dots");
                    model.AddConstr(A[i + 1, 4 * j + 3] + 1 - P[i, 2 * j] >= 1, "dots");
                    model.AddConstr(Asc[i, 4 * j + 1] + A[i + 1, 4 * j + 1] + 1 - A[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - A[i + 1, 4 * j + 1] + 1 - P[i, 2 * j] >= 1, "dots");
                    model.AddConstr(1 - Asc[i, 4 * j + 1] + A[i + 1, 4 * j + 1] + A[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(A[i + 1, 4 * j] + 1 - A[i + 1, 4 * j + 3] + P[i, 2 * j] + P[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(Asc[i, 4 * j + 1] + 1 - A[i + 1, 4 * j + 1] + A[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(Asc[i, 4 * j] + A[i + 1, 4 * j + 3] + 1 - P[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(A[i + 1, 4 * j] + A[i + 1, 4 * j + 1] + 1 - A[i + 1, 4 * j + 3] + 1 - P[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(1 - A[i + 1, 4 * j] + A[i + 1, 4 * j + 1] + 1 - A[i + 1, 4 * j + 3] + P[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(Asc[i, 4 * j] + A[i + 1, 4 * j] + A[i + 1, 4 * j + 2] + 1 - A[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - A[i + 1, 4 * j] + A[i + 1, 4 * j + 3] + 1 - P[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(1 - Asc[i, 4 * j] + A[i + 1, 4 * j] + A[i + 1, 4 * j + 2] + A[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(A[i + 1, 4 * j] + 1 - A[i + 1, 4 * j + 2] + P[i, 2 * j] + P[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(1 - P[i, 2 * j] + 1 - P[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(Asc[i, 4 * j] + 1 - A[i + 1, 4 * j] + A[i + 1, 4 * j + 2] + A[i + 1, 4 * j + 3] >= 1, "dots"); 
                }
                Pro=Pro+4*(P[i,1]+P[i,3]+P[i,5]+P[i,7])+8*(P[i,0]+P[i,2]+P[i,4]+P[i,6]);
            }
           
        //    model.AddConstr(1 - B[0, 0] + 1 - B[0, 1] + 1 - B[0, 2] + 1 - B[0, 3] + 1 - B[0, 4] + 1 - B[0, 5] + 1 - B[0, 6] + 1 - B[0, 7] + 1 - B[0, 8] + 1 - B[0, 9] + B[0, 10] + 1 - B[0, 11] + 1 - B[0, 12] + 1 - B[0, 13] + 1 - B[0, 14] + 1 - B[0, 15] >= 1, "point");
         //   model.AddConstr(1 - B[0, 0] + 1 - B[0, 1] + 1 - B[0, 2] + 1 - B[0, 3] + 1 - B[0, 4] + 1 - B[0, 5] + 1 - B[0, 6] + 1 - B[0, 7] + 1 - B[0, 8] +  B[0, 9] + 1 - B[0, 10] + 1 - B[0, 11] + 1 - B[0, 12] + 1 - B[0, 13] + 1 - B[0, 14] + 1 - B[0, 15] >= 1, "point");
            

            //model.AddConstr(B[0, 2] == 0, "dy");model.AddConstr(1 - B[0, 0] + 1 - B[0, 1] + 1 - B[0, 2] + 1 - B[0, 3] + 1-B[0, 4] + 1 - B[0, 5] + 1-B[0, 6] + 1-B[0, 7] + 1-B[0, 8] + 1 - B[0, 9] + 1 - B[0, 10] + 1 - B[0, 11] + 1 - B[0, 12] + 1 - B[0, 13] + 1 - B[0, 14] + 1 - B[0, 15] >= 1, "point");

        //    model.AddConstr(B[0,  1] == 0, "dy");
            //model.AddConstr(B[0, 4] == 0, "dy");
            //model.AddConstr(B[0, 9] == 0, "dy");
            //model.AddConstr(B[0, 13] == 0, "dy");
            // 加密过程        

            for (i = 0; i < round0; i++)
            {
                //    obj[i, 1] = 0;
                for (j = 0; j < 16; j++)
                    Dobj[i] = Dobj[i] + D[i, j];
                model.AddConstr(Dobj[i] <= 15, "5");
                model.AddConstr(Dobj[i] >= 1, "5");
                for (j = 0; j < 4; j++)
                {
                    model.AddConstr(D[i, 4 * j + 3] + 1 - Dmc[i, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - D[i, 4 * j + 3] + Dmc[i, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - Dmc[i, 4 * j] + 1 - Dmc[i, 4 * j + 1] + 1 - P3[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(D[i, 4 * j + 2] + 1 - Dmc[i, 4 * j + 2] >= 1, "dots");
                    model.AddConstr(1 - D[i, 4 * j + 2] + Dmc[i, 4 * j + 2] >= 1, "dots");
                    model.AddConstr(Dmc[i, 4 * j + 3] + 1 - P3[i, 2 * j] >= 1, "dots");
                    model.AddConstr(D[i, 4 * j + 1] + Dmc[i, 4 * j + 1] + 1 - Dmc[i, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - Dmc[i, 4 * j + 1] + 1 - P3[i, 2 * j] >= 1, "dots");
                    model.AddConstr(1 - D[i, 4 * j + 1] + Dmc[i, 4 * j + 1] + Dmc[i, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(Dmc[i, 4 * j] + 1 - Dmc[i, 4 * j + 3] + P3[i, 2 * j] + P3[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(D[i, 4 * j + 1] + 1 - Dmc[i, 4 * j + 1] + Dmc[i, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(D[i, 4 * j] + Dmc[i, 4 * j + 3] + 1 - P3[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(Dmc[i, 4 * j] + Dmc[i, 4 * j + 1] + 1 - Dmc[i, 4 * j + 3] + 1 - P3[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(1 - Dmc[i, 4 * j] + Dmc[i, 4 * j + 1] + 1 - Dmc[i, 4 * j + 3] + P3[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(D[i, 4 * j] + Dmc[i, 4 * j] + Dmc[i, 4 * j + 2] + 1 - Dmc[i, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - Dmc[i, 4 * j] + Dmc[i, 4 * j + 3] + 1 - P3[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(1 - D[i, 4 * j] + Dmc[i, 4 * j] + Dmc[i, 4 * j + 2] + Dmc[i, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(Dmc[i, 4 * j] + 1 - Dmc[i, 4 * j + 2] + P3[i, 2 * j] + P3[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(1 - P3[i, 2 * j] + 1 - P3[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(D[i, 4 * j] + 1 - Dmc[i, 4 * j] + Dmc[i, 4 * j + 2] + Dmc[i, 4 * j + 3] >= 1, "dots");     
                }
                for (j = 0; j < 16; j++)
                    model.AddConstr(Dmc[i, SC[j]] == D[i + 1, j], "");
                Pro = Pro + 4 * (P3[i, 1] + P3[i, 3] + P3[i, 5] + P3[i, 7]) + 8 * (P3[i, 0] + P3[i, 2] + P3[i, 4] + P3[i, 6]);
            }
            //for (i = 0; i < 16; i++)
            //    model.AddConstr(D[round0, i] <= A[round, i], "dy");

            //for (i = 0; i < 16; i++)
            //    model.AddConstr(B[0, i] >= A[round, i], "dy");
            //for (i = 0; i < 16; i++)
            //    model.AddConstr(B[0, i] >= D[round0, i], "dy");
            //for (i = 0; i < 16; i++)
            //    model.AddConstr(A[round, i] + D[round0, i] >= B[0, i], "dy");

            //he!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            model.AddConstr(Dobj[0] >= 1, "5");

            for (i = 0; i < 16; i++)
            {
                model.AddConstr(D[round0, i] + 1 - P4[i] >= 1, "pr");
                model.AddConstr(B[0, i] + 1 - P4[i] >= 1, "pr");
                model.AddConstr(A[round, i] + D[round0, i] + 1 - B[0, i] >= 1, "pr");
                model.AddConstr(1 - D[round0, i] + B[0, i] + P4[i] >= 1, "pr");
                model.AddConstr(1 - A[round, i] + B[0, i] + P4[i] >= 1, "pr");
                Pro = Pro + 4 * P4[i];
            }

            for (i = 0; i < round0; i++)
            {
                //  first
                for (j = 0; j < 16; j++)
                    model.AddConstr(B[i, j] == Bmc[i, SC[j]], "");
                for (j = 0; j < 4; j++)
                {
                    model.AddConstr(Bmc[i, 4 * j + 3] + 1 - B[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - Bmc[i, 4 * j + 3] + B[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - B[i + 1, 4 * j] + 1 - B[i + 1, 4 * j + 1] + 1 - P2[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(Bmc[i, 4 * j + 2] + 1 - B[i + 1, 4 * j + 2] >= 1, "dots");
                    model.AddConstr(1 - Bmc[i, 4 * j + 2] + B[i + 1, 4 * j + 2] >= 1, "dots");
                    model.AddConstr(B[i + 1, 4 * j + 3] + 1 - P2[i, 2 * j] >= 1, "dots");
                    model.AddConstr(Bmc[i, 4 * j + 1] + B[i + 1, 4 * j + 1] + 1 - B[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - B[i + 1, 4 * j + 1] + 1 - P2[i, 2 * j] >= 1, "dots");
                    model.AddConstr(1 - Bmc[i, 4 * j + 1] + B[i + 1, 4 * j + 1] + B[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(B[i + 1, 4 * j] + 1 - B[i + 1, 4 * j + 3] + P2[i, 2 * j] + P2[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(Bmc[i, 4 * j + 1] + 1 - B[i + 1, 4 * j + 1] + B[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(Bmc[i, 4 * j] + B[i + 1, 4 * j + 3] + 1 - P2[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(B[i + 1, 4 * j] + B[i + 1, 4 * j + 1] + 1 - B[i + 1, 4 * j + 3] + 1 - P2[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(1 - B[i + 1, 4 * j] + B[i + 1, 4 * j + 1] + 1 - B[i + 1, 4 * j + 3] + P2[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(Bmc[i, 4 * j] + B[i + 1, 4 * j] + B[i + 1, 4 * j + 2] + 1 - B[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(1 - B[i + 1, 4 * j] + B[i + 1, 4 * j + 3] + 1 - P2[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(1 - Bmc[i, 4 * j] + B[i + 1, 4 * j] + B[i + 1, 4 * j + 2] + B[i + 1, 4 * j + 3] >= 1, "dots");
                    model.AddConstr(B[i + 1, 4 * j] + 1 - B[i + 1, 4 * j + 2] + P2[i, 2 * j] + P2[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(1 - P2[i, 2 * j] + 1 - P2[i, 2 * j + 1] >= 1, "dots");
                    model.AddConstr(Bmc[i, 4 * j] + 1 - B[i + 1, 4 * j] + B[i + 1, 4 * j + 2] + B[i + 1, 4 * j + 3] >= 1, "dots");
                }
            
                Pro = Pro + 4 * (P2[i, 1] + P2[i, 3] + P2[i, 5] + P2[i, 7]) + 8 * (P2[i, 0] + P2[i, 2] + P2[i, 4] + P2[i, 6]);
            }
            for (i = 0; i < 16;i++ )
                ActiveS = ActiveS + B[round0, i];
            //for (j = 0; j < 16; j++)
            //    obj[0, 1] = obj[0, 1] + B[0, j];
       //     model.AddConstr(obj[0, 1] ==15, "f");
      //      model.AddConstr(B[0, 13] == 0, "dy");
                Objection = -Pro + 4 * (16 - ActiveS);
                    model.AddConstr(Objection >=1, "And");
          //          model.AddConstr(Objection ==8, "And");

                    model.SetObjective(Pro, GRB.MINIMIZE);
            // Add constraint: x + 2 y + 3 z <= 4
            //model.AddConstr(x + 2 * y + 3 * z <= 4.0, "c0");
            // Add constraint: x + y >= 1
            //model.AddConstr(x + y >= 1.0, "c1");
            // Optimize model
            model.Optimize();

            Console.WriteLine("Obj:\n value of objection=2^{-" + model.ObjVal + "}\n Probability of distinguisher=2^{-" + Pro.Value + "}\n  ActiveSbox of output=" + ActiveS.Value + "\n random prabability=2^{" + (64 - 4 * ActiveS.Value) + "}\n probability difference=2^{-" + Objection.Value + "}");

            Console.WriteLine("D:\n");
            for (i = 0; i < round0; i++)
            {
                for (j = 0; j < 4; j++)
                    Console.WriteLine(" " + D[i, j].X + " " + D[i, 4 + j].X + " " + D[i, j + 8].X + " " + D[i, j + 12].X);
                Console.WriteLine("MC");
                for (j = 0; j < 4; j++)
                    Console.WriteLine(P3[i, 2 * j].X + "," + P3[i, 2 * j + 1].X);
                for (j = 0; j < 4; j++)
                    Console.WriteLine(" " + Dmc[i, j].X + " " + Dmc[i, 4 + j].X + " " + Dmc[i, j + 8].X + " " + Dmc[i, j + 12].X);
                Console.WriteLine(" ");
                Console.WriteLine("SC");
            }
            for (j = 0; j < 4; j++)
                Console.WriteLine(" " + D[i, j].X + " " + D[i, 4 + j].X + " " + D[i, j + 8].X + " " + D[i, j + 12].X);

            Console.WriteLine("A");
            for (i = 0; i < round; i++)
            {
                for (j = 0; j < 4; j++)
                    Console.WriteLine(" " + A[i, j].X + " " + A[i, 4 + j].X + " " + A[i, j + 8].X + " " + A[i, j + 12].X);
                Console.WriteLine("-SC ");
                for (j = 0; j < 4; j++)
                    Console.WriteLine(" " + Asc[i, j].X + " " + Asc[i, 4 + j].X + " " + Asc[i, j + 8].X + " " + Asc[i, j + 12].X);
                Console.WriteLine("MC");
                for (j = 0; j < 4; j++)
                    Console.WriteLine(P[i, 2 * j].X + "," + P[i, 2 * j + 1].X);
                Console.WriteLine(" ");
            }
            for (j = 0; j < 4; j++)
                Console.WriteLine(" " + A[i, j].X + " " + A[i, 4 + j].X + " " + A[i, j + 8].X + " " + A[i, j + 12].X);
            Console.WriteLine("-SC ");
            Console.WriteLine("B:\n");
            for (i = 0; i < round0; i++)
            {
                for (j = 0; j < 4; j++)
                    Console.WriteLine(" " + B[i, j].X + " " + B[i, 4 + j].X + " " + B[i, j + 8].X + " " + B[i, j + 12].X);
                Console.WriteLine("MC");
                for (j = 0; j < 4; j++)
                    Console.WriteLine(P2[i, 2 * j].X + "," + P2[i, 2 * j + 1].X);
                for (j = 0; j < 4; j++)
                    Console.WriteLine(" " + Bmc[i, j].X + " " + Bmc[i, 4 + j].X + " " + Bmc[i, j + 8].X + " " + Bmc[i, j + 12].X);
                Console.WriteLine(" ");
                Console.WriteLine("SC");
            }
            for (j = 0; j < 4; j++)
                Console.WriteLine(" " + B[i, j].X + " " + B[i, 4 + j].X + " " + B[i, j + 8].X + " " + B[i, j + 12].X);
            Console.WriteLine("new B0:");
            for (j = 0; j < 4; j++)
                Console.WriteLine(" " + B[0, j].X + " " + B[0, 4 + j].X + " " + B[0, j + 8].X + " " + B[0, j + 12].X);
                    
            model.Dispose(); env.Dispose();
        }
        catch (GRBException e) { Console.WriteLine("Error code: " + e.ErrorCode + ". " + e.Message); }
    }
}

