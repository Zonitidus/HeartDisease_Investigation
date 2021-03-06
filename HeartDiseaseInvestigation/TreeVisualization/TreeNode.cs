﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartDiseaseInvestigation.TreeVisualization
{
    public class TreeNode
    {

        private CircleNode data;

        private TreeNode TrueNode;
        private TreeNode FalseNode;

        private float HOffset = 5;
        public float VOffset = 10;


        public float Indent = 20;
        public float SpotRadius = 5;

        private PointF DataCenter;

        public Font font = new Font("Times New Roman", 12);
        public Pen pen = Pens.Black;
        public Brush fontBrush = Brushes.Black;
        public Brush backgroundBrush = Brushes.White;

        public void SetTreeDrawingParameters(float h_offset, float v_offset, float indent, float node_radius)
        {
            HOffset = h_offset;
            VOffset = v_offset;
            Indent = indent;
            SpotRadius = node_radius;

            this.TrueNode.SetTreeDrawingParameters(h_offset, v_offset, indent, node_radius);
            this.FalseNode.SetTreeDrawingParameters(h_offset, v_offset, indent, node_radius);
        }

        public TreeNode(CircleNode new_data)
        {
            this.data = new_data;
        }
        public TreeNode(CircleNode new_data, Font font)
        {
            this.data = new_data;
            this.font = font;
        }

        public TreeNode GetTrueNode()
        {
            return this.TrueNode;
        }

        public TreeNode GetFalseNode()
        {
            return this.FalseNode;
        }

        public void AddTrueNode(TreeNode node)
        {
            this.TrueNode = node;
        }

        public void AddFalseNode(TreeNode node)
        {
            this.FalseNode = node;
        }

        public void Arrange(Graphics gr, ref float xmin, ref float ymin)
        {
            this.ArrangeHorizontally(gr, ref xmin, ref ymin);
        }

        private void ArrangeHorizontally(Graphics graph, ref float xmin, ref float ymin)
        {
            SizeF size = data.GetSize(graph, this.font);

            float x = xmin;
            float biggest_ymin = ymin + size.Height;
            float subtree_ymin = ymin + size.Height + VOffset;



            if(this.TrueNode != null)
            {
                float child_ymin = subtree_ymin;
                this.TrueNode.Arrange(graph, ref x, ref child_ymin);

                if (biggest_ymin < child_ymin) biggest_ymin = child_ymin;

                x += HOffset;
            }

            if(this.FalseNode != null)
            {
                float child_ymin = subtree_ymin;
                this.FalseNode.Arrange(graph, ref x, ref child_ymin);

                if (biggest_ymin < child_ymin) 
                    biggest_ymin = child_ymin;

                x += HOffset;
            }

            if (this.TrueNode != null || this.FalseNode != null) 
                x -= HOffset;


            float subtree_width = x - xmin;
            if (size.Width > subtree_width)
            {
                x = xmin + (size.Width - subtree_width) / 2;
                
                if(this.TrueNode != null)
                {
                    this.TrueNode.Arrange(graph, ref x, ref subtree_ymin);
                    x += HOffset;
                }

                if (this.FalseNode != null)
                {
                    this.FalseNode.Arrange(graph, ref x, ref subtree_ymin);
                    x += HOffset;
                }

                subtree_width = size.Width;
            }

            DataCenter = new PointF(
                xmin + subtree_width / 2,
                ymin + size.Height / 2);


            xmin += subtree_width;

 
            ymin = biggest_ymin;
        }

        public void DrawTree(Graphics gr, ref float x, float y)
        {
            Arrange(gr, ref x, ref y);
            DrawTree(gr);
        }

        public void DrawTree(Graphics gr)
        {
            DrawSubtreeLinks(gr);
            DrawSubtreeNodes(gr);
        }

         private void DrawSubtreeLinks(Graphics gr)
        {
            DrawSubtreeLinksHorizontal(gr);
        }

        private void DrawSubtreeLinksHorizontal(Graphics graph)
        {
            if (this.TrueNode != null)
            {
                graph.DrawLine(this.pen, DataCenter, this.TrueNode.DataCenter);

                this.TrueNode.DrawSubtreeLinksHorizontal(graph);

            }
            if (this.FalseNode != null)
            {
                graph.DrawLine(this.pen, DataCenter, this.FalseNode.DataCenter);

                this.FalseNode.DrawSubtreeLinksHorizontal(graph);
            }
        }

        // Draw the nodes for the subtree rooted at this node.
        private void DrawSubtreeNodes(Graphics graph)
        {
            this.data.Draw(DataCenter.X, DataCenter.Y, graph, this.pen, this.backgroundBrush, this.fontBrush, this.font);

            if (this.TrueNode != null)
            {
                this.TrueNode.DrawSubtreeNodes(graph);
            }
            if(this.FalseNode != null)
            {
                this.FalseNode.DrawSubtreeNodes(graph);
            }
        }
        
    }
}
