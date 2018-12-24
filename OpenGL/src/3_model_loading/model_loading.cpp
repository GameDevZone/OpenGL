#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include "../std_image.h"

#include "glm.hpp"
#include "gtc/matrix_transform.hpp"
#include "gtc/type_ptr.hpp"


#include "../Shader.h"
#include "../Camera.h"
#include "../Model.h"

#include <iostream>


#define WINDOW_WIDTH 800
#define WINDOW_HEIGHT 600

void framebuffer_size_callback(GLFWwindow *Window, int width, int height);
void processInput(GLFWwindow *window);
void mouse_callback(GLFWwindow* window, double xpos, double ypos);
void scroll_callback(GLFWwindow* window, double xoffset, double yoffset);
unsigned int loadTexture(const char *path);

// camera 
Camera camera(glm::vec3(0.f, 0.f, 3.0f));
bool firstMouse = true;
float lastX = 800.0f / 2.0;
float lastY = 600.0 / 2.0;

// delta time
float deltaTime = .0f;
float lastTime = .0f;

// light
glm::vec3 lightPos(1.2f, 1.0f, 2.0f);

bool lightRotate = false;

float shininess = 32;

glm::vec3 pointLightPositions[] = {
	glm::vec3(0.7f,  0.2f,  2.0f),
	glm::vec3(2.3f, -3.3f, -4.0f),
	glm::vec3(-4.0f,  2.0f, -12.0f),
	glm::vec3(0.0f,  0.0f, -3.0f)
};

int main(void)
{
	GLFWwindow* window;

	if (!glfwInit())
		return -1;

	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

#ifdef __APPLE__
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);
#endif // __APPLE__


	/* Create a windowed mode window and its OpenGL context */
	window = glfwCreateWindow(WINDOW_WIDTH, WINDOW_HEIGHT, "Learn OpenGL", NULL, NULL);
	if (!window)
	{
		std::cout << "Failed To Create GLFW Window" << std::endl;
		glfwTerminate();
		return -1;
	}

	glfwMakeContextCurrent(window);

	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
	{
		std::cout << "Failed to initialize GLAD" << std::endl;
		return -1;
	}

	glViewport(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);

	glfwSetInputMode(window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
	glfwSetCursorPosCallback(window, mouse_callback);
	glfwSetScrollCallback(window, scroll_callback);
	glEnable(GL_DEPTH_TEST);

	Shader shaderProgram("shader/3_section_shaders/1_model_loading.vs", "shader/3_section_shaders/1_model_loading.fs");
	Shader lampShader("shader/2_section_shaders/1_1_lamp.vs", "shader/2_section_shaders/1_1_lamp.fs");

	Model ourModel("Resources/objects/nanosuit/nanosuit.obj");
	

	unsigned int lightVAO;
	glGenVertexArrays(1, &lightVAO);
	glBindVertexArray(lightVAO);

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0);

	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindVertexArray(0);	

	float totalDelta = 0.0f;
	while (!glfwWindowShouldClose(window))
	{
		float currentFrame = (float)glfwGetTime();
		deltaTime = currentFrame - lastTime;
		lastTime = currentFrame;
		/* key input */
		processInput(window);

		/* render commands */
		glClearColor(0.2f, 0.2f, 0.2f, 1.f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		shaderProgram.Use();
		// point light 1
		shaderProgram.SetVec3("pointLights[0].position", pointLightPositions[0]);
		shaderProgram.SetVec3("pointLights[0].ambient", 0.05f, 0.05f, 0.05f);
		shaderProgram.SetVec3("pointLights[0].diffuse", 0.8f, 0.8f, 0.8f);
		shaderProgram.SetVec3("pointLights[0].specular", 1.0f, 1.0f, 1.0f);
		shaderProgram.SetFloat("pointLights[0].constant", 1.0f);
		shaderProgram.SetFloat("pointLights[0].linear", 0.09f);
		shaderProgram.SetFloat("pointLights[0].quadratic", 0.032f);
		// pointlight 2
		shaderProgram.SetVec3("pointLights[1].position", pointLightPositions[1]);
		shaderProgram.SetVec3("pointLights[1].ambient", 0.05f, 0.05f, 0.05f);
		shaderProgram.SetVec3("pointLights[1].diffuse", 0.8f, 0.8f, 0.8f);
		shaderProgram.SetVec3("pointLights[1].specular", 1.0f, 1.0f, 1.0f);
		shaderProgram.SetFloat("pointLights[1].constant", 1.0f);
		shaderProgram.SetFloat("pointLights[1].linear", 0.09f);
		shaderProgram.SetFloat("pointLights[1].quadratic", 0.032f);
		// pointlight 3
		shaderProgram.SetVec3("pointLights[2].position", pointLightPositions[2]);
		shaderProgram.SetVec3("pointLights[2].ambient", 0.05f, 0.05f, 0.05f);
		shaderProgram.SetVec3("pointLights[2].diffuse", 0.8f, 0.8f, 0.8f);
		shaderProgram.SetVec3("pointLights[2].specular", 1.0f, 1.0f, 1.0f);
		shaderProgram.SetFloat("pointLights[2].constant", 1.0f);
		shaderProgram.SetFloat("pointLights[2].linear", 0.09f);
		shaderProgram.SetFloat("pointLights[2].quadratic", 0.032f);
		// pointlight 4
		shaderProgram.SetVec3("pointLights[3].position", pointLightPositions[3]);
		shaderProgram.SetVec3("pointLights[3].ambient", 0.05f, 0.05f, 0.05f);
		shaderProgram.SetVec3("pointLights[3].diffuse", 0.8f, 0.8f, 0.8f);
		shaderProgram.SetVec3("pointLights[3].specular", 1.0f, 1.0f, 1.0f);
		shaderProgram.SetFloat("pointLights[3].constant", 1.0f);
		shaderProgram.SetFloat("pointLights[3].linear", 0.09f);
		shaderProgram.SetFloat("pointLights[3].quadratic", 0.032f);

		shaderProgram.SetVec3("spotLight.position", camera.Position);
		shaderProgram.SetVec3("spotLight.direction", camera.Front);
		shaderProgram.SetVec3("spotLight.ambient", 0.1f, 0.1f, 0.1f);
		shaderProgram.SetVec3("spotLight.diffuse", 0.5f, .5f, .5f);
		shaderProgram.SetVec3("spotLight.specular", 1.0f, 1.0f, 1.0f);
		shaderProgram.SetFloat("spotLight.constant", 1.0f);
		shaderProgram.SetFloat("spotLight.linear", 0.09f);
		shaderProgram.SetFloat("spotLight.quadratic", 0.032f);
		shaderProgram.SetFloat("spotLight.cutoff", glm::cos(glm::radians(12.5f)));
		shaderProgram.SetFloat("spotLight.outerCutoff", glm::cos(glm::radians(15.0f)));
		
		shaderProgram.SetVec3("camPos", camera.Position);

		// project matrix
		glm::mat4 projection(1.0f);
		projection = glm::perspective(glm::radians(camera.Zoom), (float)WINDOW_WIDTH / (float)WINDOW_HEIGHT, .1f, 100.f);
		glm::mat4 view = camera.GetViewMatrix();
		shaderProgram.SetMat4("projection", projection);
		shaderProgram.SetMat4("view", view);

			
		glm::mat4 model(1.0f);
		model = glm::translate(model, glm::vec3(0.0f, -1.75f, 0.0f));
		model = glm::scale(model, glm::vec3(0.2f, 0.2f, 0.2f));
		shaderProgram.SetMat4("model", model);
		ourModel.Draw(shaderProgram);


		// also draw the lamp object
		lampShader.Use();
		lampShader.SetMat4("projection", projection);
		lampShader.SetMat4("view", view);


		glBindVertexArray(lightVAO);
		for (unsigned int i = 0; i < 4; i++)
		{
			glm::mat4 model(1.0f);
			model = glm::mat4(1.f);
			model = glm::translate(model, pointLightPositions[i]);
			model = glm::scale(model, glm::vec3(0.05f));
			lampShader.SetMat4("model", model);

			ourModel.Draw(lampShader);
		}

		glfwSwapBuffers(window);
		glfwPollEvents();
	}

	glDeleteVertexArrays(1, &lightVAO);

	/* clear all the stuff */
	glfwTerminate();
	return 0;
}

void scroll_callback(GLFWwindow* window, double xoffset, double yoffset)
{
	camera.ProcessMouseScroll((float)yoffset);
}

void mouse_callback(GLFWwindow* window, double xpos, double ypos)
{
	if (firstMouse)
	{
		lastX = (float)xpos;
		lastY = (float)ypos;
		firstMouse = false;
	}

	float offsetX = (float)xpos - lastX;
	float offsetY = lastY - (float)ypos;
	lastX = (float)xpos;
	lastY = (float)ypos;

	camera.ProcessMouseMovement(offsetX, offsetY);
}

void framebuffer_size_callback(GLFWwindow *Window, int width, int height)
{
	glViewport(0, 0, width, height);
}

void processInput(GLFWwindow *window)
{
	float cameraSpeed = 2.5f * deltaTime;
	if (glfwGetKey(window, GLFW_KEY_ESCAPE))
	{
		glfwSetWindowShouldClose(window, true);
	}

	if (glfwGetKey(window, GLFW_KEY_W) == GLFW_PRESS)
		camera.ProcessKeyboard(FORWARD, deltaTime);

	if (glfwGetKey(window, GLFW_KEY_S) == GLFW_PRESS)
		camera.ProcessKeyboard(BACKWARD, deltaTime);

	if (glfwGetKey(window, GLFW_KEY_A) == GLFW_PRESS)
		camera.ProcessKeyboard(LEFT, deltaTime);

	if (glfwGetKey(window, GLFW_KEY_D) == GLFW_PRESS)
		camera.ProcessKeyboard(RIGHT, deltaTime);

	if (glfwGetKey(window, GLFW_KEY_E) == GLFW_PRESS)
		camera.ProcessKeyboard(DOWN, deltaTime);

	if (glfwGetKey(window, GLFW_KEY_Q) == GLFW_PRESS)
		camera.ProcessKeyboard(UP, deltaTime);

	if (glfwGetKey(window, GLFW_KEY_SPACE) == GLFW_PRESS)
		lightRotate = true;

	if (glfwGetKey(window, GLFW_KEY_ENTER) == GLFW_PRESS)
		lightRotate = false;

	if (glfwGetKey(window, GLFW_KEY_UP) == GLFW_PRESS)
	{
		shininess *= 2;
		if (shininess >= 512.f)
			shininess = 512.f;
	}


	if (glfwGetKey(window, GLFW_KEY_DOWN) == GLFW_PRESS)
	{
		shininess /= 2;
		if (shininess <= 8.f)
			shininess = 8.f;
	}
}

unsigned int loadTexture(char const * path)
{
	unsigned int textureID;
	glGenTextures(1, &textureID);

	int width, height, nrComponents;
	unsigned char *data = stbi_load(path, &width, &height, &nrComponents, 0);
	if (data)
	{
		GLenum format;
		if (nrComponents == 1)
			format = GL_RED;
		else if (nrComponents == 3)
			format = GL_RGB;
		else if (nrComponents == 4)
			format = GL_RGBA;

		glBindTexture(GL_TEXTURE_2D, textureID);
		glTexImage2D(GL_TEXTURE_2D, 0, format, width, height, 0, format, GL_UNSIGNED_BYTE, data);
		glGenerateMipmap(GL_TEXTURE_2D);

		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

		stbi_image_free(data);
	}
	else
	{
		std::cout << "Texture failed to load at path: " << path << std::endl;
		stbi_image_free(data);
	}

	return textureID;
}