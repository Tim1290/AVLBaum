using System;
using System.Collections.Generic;
using System.Text;

namespace AVLBaum
{
    internal class AVL<T>
    {
        //private Node rootNode;
        private Func<T, T, int> comparer;
        Node rootNode;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public AVL()
        {

        }

        public AVL(Func<T, T, int> comparer)
        {
            this.comparer = comparer;
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="data"></param>
        public void Add(T data)
        {
            Node node = rootNode;
            node = rootNode = AddRec(new Node(data), node);
            rootNode = node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        private Node AddRec(Node node, Node current)
        {
            if (current == null) 
            {
                return node;
            } 

            int compareValue = comparer(current.data, node.data);

            if (compareValue == 0) return node;

            if (compareValue < 0)
            {
                current.rightNode = AddRec(node, current.rightNode);
            }
            else
            {
                current.leftNode = AddRec(node, current.leftNode);
            }

            return current;
        }

        /// <summary>
        /// This function rotates the tree right and returns the updated root
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private Node RightRotation(Node parent)
        {
            Node node = parent.leftNode;
            parent.leftNode = node.rightNode;
            node.rightNode = parent;
            return node;
        }

        /// <summary>
        /// This function rotates the tree left and returns the updated root
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private Node LeftRotation(Node parent)
        {
            Node node = parent.rightNode;
            parent.rightNode = node.leftNode;
            node.leftNode = parent;
            return node;
        }

        /// <summary>
        /// This function rotates the tree right-left and returns the updated root
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private Node RightLeftRotation(Node parent)
        {
            parent.rightNode = RightRotation(parent.rightNode);
            return LeftRotation(parent);
        }

        /// <summary>
        /// This function rotates the tree left-right and returns the updated root
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        private Node LeftRightRotation(Node parent)
        {
            parent.leftNode = LeftRotation(parent.leftNode);
            return RightRotation(parent);
        }

        /// <summary>
        /// Tree Depth
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private int getHeight(Node current)
        {
            int height = 0;
            if (current != null)
            {
                int left = getHeight(current.leftNode);
                int right = getHeight(current.rightNode);
                int maxHeight = max(left, right);
                height = maxHeight + 1;
            }
            return height;
        }

        /// <summary>
        /// Balance Factor
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private int BalanceFactor(Node current)
        {
            if(current == null)
            {
                return 0;
            }
            return getHeight(current.leftNode) - getHeight(current.rightNode);
        }

        /// <summary>
        /// Check if the tree is unbalanced
        /// </summary>
        /// <returns></returns>
        private bool IsUnbalanced()
        {
            int balanceFactor = BalanceFactor(rootNode);

            bool isUnbalanced = balanceFactor > 1 || balanceFactor < -1;

            return isUnbalanced;
        }

        /// <summary>
        /// 
        /// </summary>
        private void Balance()
        {
            while (IsUnbalanced())
            {
                rootNode = BalanceTree(rootNode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ContainsRec(T value, Node node)
        {
            if(node == null)
            {
                return false;
            }

            int compare = comparer(node.data, value);

            if(compare == 0)
            {
                return true;
            }

            if(compare < 0)
            {
                return ContainsRec(value, node.rightNode);
            }
            else
            {
                return ContainsRec(value, node.leftNode);
            }
        }

        public bool Contains(T data)
        {
            return ContainsRec(data, rootNode);
        }

        /// <summary>
        /// Max Tree Size
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private int max(int left, int right)
        {
            return left > right ? left : right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Node BalanceTree(Node node)
        {
            if(node == null)
            {
                return node;
            }

            node.leftNode = BalanceTree(node.leftNode);
            node.rightNode = BalanceTree(node.rightNode);

            int bFactor = BalanceFactor(node);
            if (bFactor > 1)
            {
                if (BalanceFactor(node.leftNode) > 0)
                {
                    node = RightRotation(node);
                }
                else
                {
                    node = LeftRightRotation(node);
                }
            }
            else if (bFactor < -1)
            {
                if (BalanceFactor(node.rightNode) > 0)
                {
                    node = LeftRotation(node);
                }
                else
                {
                    node = RightLeftRotation(node);
                }
            }
            return node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Remove(T data)
        {
            rootNode = RemoveRec(data, rootNode);
            Balance();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private Node RemoveRec(T data, Node node)
        {
            if(node == null)
            {
                return node;
            }

            int compare = comparer(node.data, data);

            if(compare == 0)
            {
                if(node.rightNode == null && node.leftNode == null)
                {
                    return null;
                }
                return node.leftNode;
            }

            else if(node.leftNode == null) { return node.rightNode; }

            if(BalanceFactor(node) < 0)
            {
                AddRec(node.leftNode, node.rightNode);
                return node.rightNode;
            }
            if(BalanceFactor(node) > 0)
            {
                AddRec(node.rightNode, node.leftNode);
                return node.leftNode;
            }

            if(compare < 0) { node.rightNode = RemoveRec(data, node.rightNode); }
            else { node.leftNode = RemoveRec(data, node.leftNode); }

           
            return node;
        }

        /// <summary>
        /// Node Class
        /// </summary>
        private class Node
        {
            public Node(T data)
            {
                this.data = data;
            }

            public T data;
            public Node leftNode;
            public Node rightNode;
        }
    }
}
