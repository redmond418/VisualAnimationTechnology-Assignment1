#ifndef CUSTOM_PERLIN_NOISE_3D_INCLUDED
#define CUSTOM_PERLIN_NOISE_3D_INCLUDED

float Hash(float3 p)
{
    return frac(sin(dot(p, float3(127.1, 311.7, 74.7))) * 43758.5453);
}

float3 PerlinGradient3D(float3 p)
{
    float h = Hash(p);
    return normalize(float3(frac(h * 123.4), frac(h * 456.7), frac(h * 789.1)) * 2.0 - 1.0);
}

void PerlinNoise3D_float(float3 Position, out float Result)
{
    float3 Pi = floor(Position);
    float3 Pf = frac(Position);
    
    float3 f = Pf * Pf * Pf * (Pf * (Pf * 6.0 - 15.0) + 10.0);

    float3 p000 = Pi + float3(0, 0, 0);
    float3 p100 = Pi + float3(1, 0, 0);
    float3 p010 = Pi + float3(0, 1, 0);
    float3 p110 = Pi + float3(1, 1, 0);
    float3 p001 = Pi + float3(0, 0, 1);
    float3 p101 = Pi + float3(1, 0, 1);
    float3 p011 = Pi + float3(0, 1, 1);
    float3 p111 = Pi + float3(1, 1, 1);

    float n000 = dot(PerlinGradient3D(p000), Pf - float3(0, 0, 0));
    float n100 = dot(PerlinGradient3D(p100), Pf - float3(1, 0, 0));
    float n010 = dot(PerlinGradient3D(p010), Pf - float3(0, 1, 0));
    float n110 = dot(PerlinGradient3D(p110), Pf - float3(1, 1, 0));
    float n001 = dot(PerlinGradient3D(p001), Pf - float3(0, 0, 1));
    float n101 = dot(PerlinGradient3D(p101), Pf - float3(1, 0, 1));
    float n011 = dot(PerlinGradient3D(p011), Pf - float3(0, 1, 1));
    float n111 = dot(PerlinGradient3D(p111), Pf - float3(1, 1, 1));

    float nx00 = lerp(n000, n100, f.x);
    float nx10 = lerp(n010, n110, f.x);
    float nx01 = lerp(n001, n101, f.x);
    float nx11 = lerp(n011, n111, f.x);

    float nxy0 = lerp(nx00, nx10, f.y);
    float nxy1 = lerp(nx01, nx11, f.y);

    Result = lerp(nxy0, nxy1, f.z);
}

#endif
