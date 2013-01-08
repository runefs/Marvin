using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Marvin.Examples.Dijkstra___Manhattan
{
    
    public class Subgraph<TNode>
    {
        public Subgraph(TNode node, IEnumerable<KeyValuePair<TNode,IDictionary<TNode,double>>> graph) : this(node,new Graph(graph)){
        }

        private Subgraph(TNode node, Graph graph) {
            this.node = node;
            this.graph = graph;
            this.graph.UpdateDistance(node,0.0);
        }
        
        role graph : Graph {
             double DistanceBetween(TNode node, TNode other){
                   return self.Nodes[node][other];
             }

            void MarkNodeAsVisited(){
                graph.Nodes.Remove(node);
            }

            void UpdateDistance(TNode n,double distance){
                    self.Distances[n] = distance;
            }
        }

        role node : TNode
        {
            IDictionary<TNode,double> Neighbors(){
                    return graph.Nodes[self];
            } 

            double Distance(){
                return graph.Distances[self];
            }

           void IsPreviousOf(TNode n){
                if(!graph.Previous.ContainsKey(n)){
                    graph.Previous.Add(n,default(TNode));
                }
                graph.Previous[n] = self;
            }

            double DistanceTo(TNode other){
                return graph.DistanceBetween(self,other);
            }

            TNode NeighborWithShortestPath(){
                 return graph.Distances.Where(n=> graph.Nodes.ContainsKey(n.Key))
                                       .OrderBy(n => n.Value).FirstOrDefault().Key;
            }
        }

        interaction Dictionary<TNode,TNode> FindShortestPath() {
            var neighbors = node.Neighbors().Keys;
            if(!neighbors.Any()) return graph.Previous;

            foreach(var v in  neighbors) {
                var alt = node.Distance() + node.DistanceTo(v);
                if(alt < graph.Distances[v]) { 
                    graph.UpdateDistance(v,alt);
                    node.IsPreviousOf(v);
                }
            }
            graph.MarkNodeAsVisited();
            var u = node.NeighborWithShortestPath();
            return new Subgraph<TNode>(u, graph).FindShortestPath();
        }
       
      class Graph {
            private readonly Dictionary<TNode,double> _dist = new Dictionary<TNode,double>();
            private readonly Dictionary<TNode,TNode> _previous = new Dictionary<TNode,TNode>(); 
            private readonly IDictionary<TNode,IDictionary<TNode,double>> _nodes;
            public Graph(IEnumerable<KeyValuePair<TNode,IDictionary<TNode,double>>> graph){
                 _nodes = (graph as IDictionary<TNode,IDictionary<TNode,double>>);
                if(_nodes == null){
                   _nodes = new Dictionary<TNode,IDictionary<TNode,double>>();
                   foreach(var pair in _nodes) {
                       _nodes.Add(pair.Key,pair.Value);
                   }
                }
                              
                foreach(var node in _nodes.Keys){
                    _dist.Add(node,double.PositiveInfinity);
                }
            }

            public IDictionary<TNode,IDictionary<TNode,double>> Nodes{
                get { return _nodes;}
            }
            
            public Dictionary<TNode,double> Distances{
                get{ return _dist;}
            }

            public Dictionary<TNode,TNode> Previous{
                get{ return _previous;}
            }
        }
    }
}
