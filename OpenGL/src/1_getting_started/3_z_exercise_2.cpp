#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <iostream>

#include "../Shader.h"

// window size change call back
void framebuffer_size_callback(GLFWwindow* window, int width, int height);

// input process
void processInput(GLFWwindow *window);

// settings
const unsigned int SCR_WIDTH = 800;
const unsigned int SCR_HEIGHT = 600;


int main()
{
	glfwInit();
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

#ifdef __APPLE__
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
#endif

	GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "LearnOpenGL", NULL, NULL);
	if (window == NULL)
	{
		std::cout << "Failed to create GLFW window" << std::endl;
		glfwTerminate();
		return -1;
	}
	glfwMakeContextCurrent(window);
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
		std::cout << "Failed to initialize GLAD" << std::endl;
		return -1;
	}

	Shader shaderProgram("shader/1_section_shaders/3_z_exercise2_v.vs", "shader/1_section_shaders/3_z_exercise2_f.fs");

	// triangle
	float vertices[] = {
		-0.5f, -0.5f, 0.0f, 1.0f, .0f, .0f,
		0.5f, -0.5f, 0.0f,  .0f, 1.0f, .0f,
		0.0f,  0.5f, 0.0f,  .0f, .0f, 1.0f
	};

	unsigned int VBO, VAO;
	glGenVertexArrays(1, &VAO);

	glBindVertexArray(VAO);

	glGenBuffers(1, &VBO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(3 * sizeof(float)));
	glEnableVertexAttribArray(1);

	// unbind vao
	glBindVertexArray(0);

	shaderProgram.Use();

	float value = 0.0f;
	float offset = .05f;

	// render loop
	while (!glfwWindowShouldClose(window))
	{
		processInput(window);

		if (value > 1.0f)
			offset =  -0.05f;

		if (value < -1.0f)
			offset = 0.05f;

		value += offset;

		glClearColor(0.2f, .2f, .2f, 1);
		glClear(GL_COLOR_BUFFER_BIT);

		glUniform1f(glGetUniformLocation(shaderProgram.ShaderID, "xOffset"), value);

		glBindVertexArray(VAO);
		glDrawArrays(GL_TRIANGLES, 0, 3);

		glfwSwapBuffers(window);
		glfwPollEvents();
	}

	// glfw: terminate
	glfwTerminate();

	return 0;
}


void framebuffer_size_callback(GLFWwindow* window, int width, int height)
{
	glViewport(0, 0, width, height);
}

// process all input
void processInput(GLFWwindow *window)
{
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
		glfwSetWindowShouldClose(window, true);
}