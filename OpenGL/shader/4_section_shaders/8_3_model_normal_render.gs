#version 330 core
layout(triangles) in;
layout(line_strip, max_vertices = 6) out;

const float MAGNITUDE = 0.4;

in VS_OUT
{
	vec3 Normal;
}gs_in[];

uniform float times;

vec3 GetNormal()
{
	vec3 a = vec3(gl_in[0].gl_Position) - vec3(gl_in[1].gl_Position);
	vec3 b = vec3(gl_in[2].gl_Position) - vec3(gl_in[1].gl_Position);
	return normalize(cross(a,b));
}


vec4 explode(vec4 position, vec3 normal)
{
	float magnitude = 2.0f;
	vec3 direction = normal * magnitude * ((sin(times) + 1.0) / 2.0);
	return position + vec4(direction, 0.0);
}

void GenerateLine(int index, vec3 normal)
{
	vec4 pos = explode(gl_in[index].gl_Position, normal);
	gl_Position = pos;
	EmitVertex();
	gl_Position = pos + vec4(gs_in[index].Normal, 0.0) * MAGNITUDE;
	EmitVertex();
	EndPrimitive();
}

void main()
{	
	vec3 normal = GetNormal();
	GenerateLine(0, normal);
	GenerateLine(1, normal);
	GenerateLine(2, normal);
}
