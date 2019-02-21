#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTexcoord;

//uniform vec3 lightPos;

uniform mat4 view;
uniform mat4 projection;
uniform mat4 model;

out vec4 lightPosScreenSpace;
out vec2 TexCoord;

void main()
{
	lightPosScreenSpace = projection * view * model * vec4(0,0,0, 1.0);
	TexCoord = aTexcoord;
	gl_Position = vec4(aPos, 1);
}