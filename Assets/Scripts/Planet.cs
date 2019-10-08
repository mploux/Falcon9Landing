using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planet : MonoBehaviour {

	List<Vector3> vertices = new List<Vector3>();
	List<Vector3> normals = new List<Vector3>();
	List<Triangle> triangles = new List<Triangle>();

	private float radius = 1;

	void Start () {
		generate (2);

		MeshFilter meshFilter = GetComponent<MeshFilter>();
		meshFilter.mesh = createMesh();
		MeshRenderer renderer = GetComponent<MeshRenderer>();
		renderer.material.color = Color.green;
	}

	private void generate(int iteration) {
		float t = (float)((1.0 + Mathf.Sqrt(5.0f)) / 2.0f);
		
		vertices.Add(new Vector3(-1 * radius, t * radius, 0));
		vertices.Add(new Vector3(0, 1 * radius, -t * radius));
		vertices.Add(new Vector3(-t * radius, 0, -1 * radius));
		vertices.Add(new Vector3(0, -1 * radius, -t * radius));
		vertices.Add(new Vector3(-1 * radius, -t * radius, 0));
		vertices.Add(new Vector3(1 * radius, -t * radius, 0));
		vertices.Add(new Vector3(0, -1 * radius, t * radius));
		vertices.Add(new Vector3(t * radius, 0, 1 * radius));
		vertices.Add(new Vector3(0, 1 * radius, t * radius));
		vertices.Add(new Vector3(1 * radius, t * radius, 0));
		vertices.Add(new Vector3(t * radius, 0, -1 * radius));
		vertices.Add(new Vector3(-t * radius, 0, 1 * radius));
		
		triangles.Add(new Triangle(0, 1, 2));
		triangles.Add(new Triangle(0, 2, 11));
		triangles.Add(new Triangle(0, 11, 8));
		triangles.Add(new Triangle(0, 8, 9));
		triangles.Add(new Triangle(0, 9, 1));
		triangles.Add(new Triangle(2, 4, 11));
		triangles.Add(new Triangle(11, 4, 6));
		triangles.Add(new Triangle(8, 11, 6));
		triangles.Add(new Triangle(8, 6, 7));
		triangles.Add(new Triangle(8, 7, 9));
		triangles.Add(new Triangle(7, 10, 9));
		triangles.Add(new Triangle(7, 5, 10));
		triangles.Add(new Triangle(10, 5, 3));
		triangles.Add(new Triangle(1, 10, 3));
		triangles.Add(new Triangle(2, 3, 4));
		triangles.Add(new Triangle(3, 5, 4));
		triangles.Add(new Triangle(6, 5, 7));
		triangles.Add(new Triangle(6, 4, 5));
		triangles.Add(new Triangle(9, 10, 1));
		triangles.Add(new Triangle(2, 1, 3));

		List<Triangle> oldTriangles = new List<Triangle>();
		List<int> addedVertices = new List<int>();
		
		for(int i = 0; i < vertices.Count; i++)
			addedVertices.Add(i);
		
		for(int iterations = 0; iterations < iteration; iterations++)
		{
			oldTriangles.Clear();

			for (int i = 0; i < triangles.Count; i++) {
				oldTriangles.Add(triangles[i]);
			}
			triangles.Clear();
			
			for(int i = 0; i < oldTriangles.Count; i++)
				subdivideTriangle(oldTriangles[i], addedVertices);
			
			for(int i = 0; i < addedVertices.Count; i++)
			{
				vertices[addedVertices[i]] = vertices[addedVertices[i]].normalized;
/*				Vector3 v = vertices[addedVertices[i]];
				
				float length = v.magnitude;
				
				v.x /= length;
				v.y /= length;
				v.z /= length;

				v.x *= radius;
				v.y *= radius;
				v.z *= radius;*/
			}
		}
		
		normals.Clear();
		
		for(int i = 0; i < vertices.Count; i++)
		{
			Vector3 v = vertices[i];
			
			double length = v.magnitude;
			
			normals.Add(new Vector3((float)(v.x / length), (float)(v.y / length), (float)(v.z / length)));
		}
	}

	private void subdivideTriangle(Triangle t, List<int> addedVertices) {
		Triangle t1, t2, t3, t4;
		int v1, v2, v3;
		
		v1 = getMiddleVertexIndice(t.a, t.b, addedVertices);
		v2 = getMiddleVertexIndice(t.b, t.c, addedVertices);
		v3 = getMiddleVertexIndice(t.c, t.a, addedVertices);
		
		t1 = new Triangle(t.a, v1, v3);
		t2 = new Triangle(t.b, v2, v1);
		t4 = new Triangle(t.c, v3, v2);
		t3 = new Triangle(v1, v2, v3);
		
		triangles.Add(t1);
		triangles.Add(t2);
		triangles.Add(t3);
		triangles.Add(t4);
	}
	
	private int getMiddleVertexIndice(int i1, int i2, List<int> addedVertices) {
		Vector3 v1 = vertices[i1];
		Vector3 v2 = vertices[i2];
		
		Vector3 middle = new Vector3((v1.x + v2.x) / 2.0f, (v1.y + v2.y) / 2.0f, (v1.z + v2.z) / 2.0f);
		
		int indice;
		if((indice = vertexExists(middle, addedVertices)) != -1)
		{
			return indice;
		}
		
		vertices.Add(middle);
		addedVertices.Add(vertices.Count - 1);
		return vertices.Count - 1;
	}
	
	private int vertexExists(Vector3 vertex, List<int> addedVertices) {
		for(int i = 0; i < addedVertices.Count; i++) {
			if(vertices[addedVertices[i]].x == vertex.x &&
			   vertices[addedVertices[i]].y == vertex.y &&
			   vertices[addedVertices[i]].z == vertex.z) {
				return i;
			}
		}
		
		return -1;
	}


	private Mesh createMesh() {
		Vector3[] vertices = new Vector3[this.vertices.Count];
		Vector3[] normals = new Vector3[this.normals.Count];
		int[] indices = new int[this.triangles.Count * 3];

		for(int i = 0; i < vertices.Length; i++) {
			vertices[i] = this.vertices[i];
		}

		for(int i = 0; i < normals.Length; i++) {
			normals[i] = this.normals[i];
		}

		for(int i = 0; i < indices.Length; i += 3) {
			indices[i + 0] = this.triangles[i / 3].a;
			indices[i + 1] = this.triangles[i / 3].b;
			indices[i + 2] = this.triangles[i / 3].c;
		}

		Mesh m = new Mesh ();

		m.vertices = vertices;
		m.normals = normals;
		m.triangles = indices;

		return m;
	}

	class Triangle {
		public int a, b, c;

		public Triangle(int a, int b, int c) {
			this.a = a;
			this.b = b;
			this.c = c;
		}
 	}
}