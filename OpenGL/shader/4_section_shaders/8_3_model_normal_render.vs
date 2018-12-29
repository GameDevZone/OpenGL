#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;

out VS_OUT{
	vec3 Normal;
}vs_out;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{  
	vec3 Normal = mat3(transpose(inverse(model))) * aNormal;
	vec3 ModelPos = vec3(model * vec4(aPos, 1.0));
	vs_out.Normal = normalize(Normal);
    gl_Position = projection * view * model * vec4(aPos, 1.0);
}