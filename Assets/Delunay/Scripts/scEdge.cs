using UnityEngine;
public class scEdge {

	private scVertexNode node0;
	private scVertexNode node1;
	
	private Color theDrawColor = new Color(255,0,0,1);
	private LineRenderer theLine;
	
	public scEdge(scVertexNode _n0, scVertexNode _n1){
		node0 = _n0;
		node1 = _n1;
		theLine = new GameObject().AddComponent<LineRenderer>();
		theLine.material = new Material (Shader.Find("Particles/Additive"));
		theLine.name = "EdgeLine";
		theLine.tag = "Lines";
		//theLine.renderer.material.color = theDrawColor;
	}
	
	public scVertexNode getNode0(){
		return node0;
	}
	
	public scVertexNode getNode1(){
		return node1;	
	}
	
	public bool checkSame(scEdge _aEdge){
		if 	( (node0 == _aEdge.getNode0() || node0 == _aEdge.getNode1()) &&
			  (node1 == _aEdge.getNode0() || node1 == _aEdge.getNode1())){
			return true;
		}
		
		return false;
	}
	
	public bool edgeContainsVertex(scVertexNode _aNode){
		if (node0 == _aNode || node1 == _aNode){
			return true;	
		}
		
		return false;
	}
	
	public void drawEdge(){
		if(node0.getParentCell() != null && node1.getParentCell() != null){
			if (theLine == null){
				theLine = new GameObject().AddComponent<LineRenderer>();
				theLine.name = "EdgeLine";
				theLine.material = new Material (Shader.Find("Particles/Additive"));
				theLine.tag = "Lines";
			}
			
			theLine.SetWidth(0.7f, 0.7f);
			//theLine.renderer.material.color = theDrawColor;
			theLine.SetColors(theDrawColor,theDrawColor);
			theLine.SetVertexCount(2);
			theLine.SetPosition(0, new Vector3(node0.getVertexPosition().x, node0.getVertexPosition().y,-3));
			theLine.SetPosition(1,new Vector3(node1.getVertexPosition().x, node1.getVertexPosition().y,-3));
		}
	}
	
	public void setDrawColor(Color _theColor){
		theDrawColor = new Color(_theColor.r,_theColor.g,_theColor.b,1);
		if (theLine != null){
			//theLine.renderer.material.color = theDrawColor;
			theLine.material = new Material (Shader.Find("Particles/Additive"));
		}
	}
	
	public void stopDraw(){
		if (theLine != null){
			GameObject.Destroy(theLine.gameObject);	
		}
	}
	
	

}
