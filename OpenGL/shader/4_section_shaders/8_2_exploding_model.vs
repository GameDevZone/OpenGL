#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

out VS_OUT{
	vec2 TexCoords;
	vec3 Normal;
	vec3 ModelPos;
}vs_out;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{  
	vec3 Normal = mat3(transpose(inverse(model))) * aNormal;
	vec3 ModelPos = vec3(model * vec4(aPos, 1.0));
	vs_out.TexCoords = aTexCoords;
	vs_out.Normal = Normal;
	vs_out.ModelPos = ModelPos;
    gl_Position = projection * view * model * vec4(aPos, 1.0);
}