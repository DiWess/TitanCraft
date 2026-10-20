// The harbour sea east of the quarter wall (workstream D). Three summed sine
// swells in world space, so tiles of the plane meet seamlessly, with a normal
// from their analytic derivatives, a fresnel sheen and pale crests. Opaque
// and unlit-cheap on purpose: it is a backdrop the player looks across, not
// a surface they enter (the quarter's boundary stops them at the wall).
shader_type spatial;
render_mode cull_back, depth_draw_opaque, specular_schlick_ggx;

uniform vec4 deep_color : source_color = vec4(0.035, 0.14, 0.19, 1.0);
uniform vec4 shallow_color : source_color = vec4(0.10, 0.33, 0.37, 1.0);
uniform vec4 crest_color : source_color = vec4(0.80, 0.84, 0.82, 1.0);
uniform float wave_height = 0.14;
uniform float wave_speed = 0.55;
uniform float shore_x = 36.0;

varying float swell;
varying float shore;

void vertex() {
	vec3 w = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	float t = TIME * wave_speed;
	float a = w.x * 0.35 + t;
	float b = w.z * 0.27 - t * 1.3;
	float c = (w.x + w.z) * 0.8 + t * 1.7;
	float h = sin(a) * 0.6 + sin(b) * 0.4 + sin(c) * 0.15;
	VERTEX.y += h * wave_height;
	swell = h;
	shore = clamp((w.x - shore_x) / 30.0, 0.0, 1.0);
	float dx = cos(a) * 0.35 * 0.6 + cos(c) * 0.8 * 0.15;
	float dz = cos(b) * 0.27 * 0.4 + cos(c) * 0.8 * 0.15;
	NORMAL = normalize(vec3(-dx * wave_height, 1.0, -dz * wave_height));
}

void fragment() {
	float fresnel = pow(1.0 - clamp(dot(NORMAL, VIEW), 0.0, 1.0), 4.0);
	vec3 water = mix(shallow_color.rgb, deep_color.rgb, shore);
	water = mix(water, crest_color.rgb, smoothstep(0.8, 1.05, swell) * 0.3);
	ALBEDO = water;
	ROUGHNESS = 0.07;
	METALLIC = 0.0;
	SPECULAR = 0.6;
	EMISSION = water * 0.04 + fresnel * vec3(0.04, 0.06, 0.07);
}
