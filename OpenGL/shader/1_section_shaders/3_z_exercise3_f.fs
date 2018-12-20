 #version 330 core  
 out vec4 FragColor;
 
 in vec3 ourColor; 
 in vec3 verPos;
 
 void main()  
 {  
 	FragColor = vec4(verPos, 1);  
 };