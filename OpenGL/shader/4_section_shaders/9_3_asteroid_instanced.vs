#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 2) in vec2 aTexCoords;
layout (location = 3) in mat4 instancedMatrix;

uniform mat4 view;
uniform mat4 projection;

out vec2 TexCoords;

void main()
{
    TexCoords = aTexCoords;
    gl_Position = projection * view * instancedMatrix * vec4(aPos, 1.0);
}  
