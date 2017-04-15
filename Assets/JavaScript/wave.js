 #pragma strict
 
 var scale = 10.0;
 var speed = 1.0;
 var noiseStrength = 4.0;
 
 private var baseHeight : Vector3[];
 
 function Update () {
     var mesh : Mesh = GetComponent(MeshFilter).mesh;

     if (baseHeight == null)
     {
     	 baseHeight = mesh.vertices;
	 }
     var vertices = new Vector3[baseHeight.Length];
     for (var i=0;i<vertices.Length;i++)
     {
         var vertex = baseHeight[i];
         vertex.y += Mathf.Sin(Time.time * speed+ baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
         vertex.y += Mathf.PerlinNoise(baseHeight[i].x + 4, baseHeight[i].y + Mathf.Sin(Time.time * 0.1)    ) * noiseStrength;
         vertices[i] = vertex;
     }
     mesh.vertices = vertices;
     mesh.RecalculateNormals();

 }