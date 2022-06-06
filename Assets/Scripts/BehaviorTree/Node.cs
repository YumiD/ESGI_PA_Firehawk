using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class Node
    {
        protected NodeState State;

        public Node Parent { get; private set; }
        protected List<Node> Children = new List<Node>();

        private readonly Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        protected Node()
        {
            Parent = null;
        }

        protected Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                Attach(child);
            }
        }

        private void Attach(Node node)
        {
            node.Parent = this;
            Children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.FAILURE;

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public void SetDataToRoot(string key, object value)
        {
            Node node = Parent;
            while (node.Parent != null)
            {
                node = node.Parent;
            }

            node._dataContext[key] = value;
        }

        protected object GetData(string key)
        {
            if (_dataContext.TryGetValue(key, out var value))
            {
                return value;
            }

            return Parent?.GetData(key);
        }

        protected bool ClearData(string key)
        {
            if (_dataContext.TryGetValue(key, out var value))
            {
                _dataContext.Remove(key);
                return true;
            }

            var node = Parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.Parent;
            }

            return false;
        }

        protected List<Node> ShuffleList(List<Node> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var temp = list[i];
                var randomIndex = Random.Range(i, Children.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }

            return list;
        }
    }
}