#version 450 core

in vec4 frag_color;
in vec2 frag_tex_coord;
in vec3 frag_normal;
in vec3 frag_position;

uniform sampler2D texture0;
uniform vec3 light_color;
uniform vec3 light_position;
uniform vec3 camera_position;

out vec4 color;

void main()
{
	float ambient_strength = 0.2;
	vec3 ambient = light_color * ambient_strength;

	vec3 norm = normalize(frag_normal);
	vec3 light_direction = normalize(light_position - frag_position);
	float diff = max(dot(norm, light_direction), 0.0);
	vec3 diffuse = light_color * diff;

	float specular_strength = 0.5;
	vec3 view_direction = normalize(camera_position - frag_position);
	vec3 reflection_direction = reflect(-light_direction, norm);
	float spec = pow(max(dot(view_direction, reflection_direction), 0.0), 32);
	vec3 specular = light_color * specular_strength * spec;

	vec3 light = ambient + diffuse + specular;
	color = vec4(light, 1.0) * texture(texture0, frag_tex_coord) * frag_color;
}