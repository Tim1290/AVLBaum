using System;
using System.Collections.Generic;
using System.Text;

namespace AVLBaum
{
    internal class AVL<T>
    {
        public AVL(Func<T, T, int> comparer)
        {
            this.comparer = comparer;
        }

        private Func<T, T, int> comparer;
        private Node rootNode;

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
                if (BalanceFactor(node.rightNode) < 0)
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
            rootNode = RemoveRecursive(data, rootNode);
            Balance();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private Node RemoveRecursive(T dataToRemove, Node current)
        {
            if (current == null) return null;

            int compareValue = comparer(current.data, dataToRemove);

            if (compareValue == 0)
            {
                // Captures all cases where there aren't 2 childnodes
                if (current.rightNode == null)
                {
                    if (current.leftNode == null)
                    {
                        return null;
                    }

                    return current.leftNode;
                }
                else if (current.leftNode == null)
                {
                    return current.rightNode;
                }

                // Replace root with the Node with higher depth while adding the node with lower depth to it
                if (BalanceFactor(current) > 0)
                {
                    AddRec(current.rightNode, current.leftNode);
                    return current.leftNode;
                }
                else
                {
                    AddRec(current.leftNode, current.rightNode);
                    return current.rightNode;
                }
            }

            if (compareValue < 0)
            {
                current.rightNode = RemoveRecursive(dataToRemove, current.rightNode);
            }
            else
            {
                current.leftNode = RemoveRecursive(dataToRemove, current.leftNode);
            }

            return current;
        }

        /// <summary>
        /// Rotates right and returns the new root of that rotation
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private Node RightRotation(Node root)
        {
            Node newRoot = root.leftNode;
            root.leftNode = newRoot.rightNode;
            newRoot.rightNode = root;
            return newRoot;
        }

        /// <summary>
        /// Rotates left and returns the new root of that rotation
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private Node LeftRotation(Node root)
        {
            Node newRoot = root.rightNode;
            root.rightNode = newRoot.leftNode;
            newRoot.leftNode = root;
            return newRoot;
        }

        /// <summary>
        /// Rotates left-right and returns the new root of that rotation
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private Node LeftRightRotation(Node root)
        {
            root.leftNode = LeftRotation(root.leftNode);
            return RightRotation(root);
        }

        /// <summary>
        /// Rotates right-left and returns the new root of that rotation
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private Node RightLeftRotation(Node root)
        {
            root.rightNode = RightRotation(root.rightNode);
            return LeftRotation(root);
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
