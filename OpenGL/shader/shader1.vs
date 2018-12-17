#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aColor;

uniform float offset;

out vec3 ourColor;
out vec3 verPos;

void main()
{
	//gl_Position = vec4(-aPos.x, -aPos.y, aPos.z, 1.0f); // exercise 1
	//gl_Position = vec4(aPos.x + offset, aPos.y, aPos.z, 1.0f);	// exercise 2
	gl_Position = vec4(aPos, 1.0f);
	verPos = aPos;
	ourColor = aColor;
}
