using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Spacecraft : MonoBehaviour
{
    //[SerializeField] float e;
    //[SerializeField] float a;
    [SerializeField] Transform primaryTransform;
    [SerializeField] Text apoapsisText;
    [SerializeField] Text periapsisText;
    [SerializeField] Slider EccentricitySlider;
    [SerializeField] Slider SmaSlider;

    // states
    float trueAnomaly; // in rad
    float[] rVect = { 0, 0 }; // km
    float rPeriapsis;
    float rApoapsis;
    float p;
    float a;
    float e;

    // constants
    float muEarth = 398600; // km2/s2 
    float muEarthDU = 1; // DU3/TU2
    int radiusEarth = 6378; // km

    // cached references
    GameSession gameSession;
    LineRenderer lineRenderer;
    UIManager UIManagerRef;

    // conversions
    float TU2sec = 806.78f;
    float km2units = 0.001f;

    void Start()
    {
        // set cached refs
        gameSession = FindObjectOfType<GameSession>();
        lineRenderer = GetComponent<LineRenderer>();
        UIManagerRef = FindObjectOfType<UIManager>();

        // init states
        trueAnomaly = 0f;
        a = SmaSlider.value;
        e = EccentricitySlider.value;
        p = a * (1 - e * e);
        rPeriapsis = a * (1 - e);
        rApoapsis = a * (1 + e);
        primaryTransform.position = new Vector2(e * a * km2units, 0);

        // init position vector at periapsis
        rVect[0] = rPeriapsis;
        rVect[1] = 0f;
    }

    void Update()
    {
        // update spacecraft states
        p = a * (1 - e * e);
        rPeriapsis = a * (1 - e);
        rApoapsis = a * (1 + e);
        UpdateTrueAnomaly();
        UpdateSpacecraftPos();
        UpdateOrbitTray();

        // update primary position
        primaryTransform.position = new Vector2(e * a * km2units, 0);

        // update GUI
        UIManagerRef.UpdateApoapsisText(rApoapsis - radiusEarth);
        UIManagerRef.UpdatePeriapsisText(rPeriapsis - radiusEarth);
        UIManagerRef.UpdateSmaText(a);
        UIManagerRef.UpdateEccentricityText(e);
    }

    private void UpdateSpacecraftPos()
    {
        float rScalar = p / (1 + e * Mathf.Cos(trueAnomaly));

        rVect[0] = rScalar * Mathf.Cos(trueAnomaly) + e * a;
        rVect[1] = rScalar * Mathf.Sin(trueAnomaly);

        transform.position = new Vector2(rVect[0] * km2units, rVect[1] * km2units);
    }

    private void UpdateTrueAnomaly()
    {
        // circular and elliptic case
        if (e >= 0 && e < 1)
        {
            float ellipticalEccentricAnomaly = SolveEllipticInverseKeplerProblem();
            trueAnomaly = E2ni(ellipticalEccentricAnomaly);
        }

        // parabolic case
        // TODO

        // hyperbolic case
        // TODO
    }

    // consider deltaT = time from start
    private float SolveEllipticInverseKeplerProblem()
    {
        float newE, oldE, M;
        float tol = 0.001f;

        M = Mathf.Sqrt(muEarth / Mathf.Pow(a, 3)) * Time.timeSinceLevelLoad; // gameSession.GetTimeFromPeriapsis(); // * gameSession.GetTimeWarp();
        newE = M;
        int i = 0;

        do
        {
            oldE = newE;
            newE = oldE + (M - oldE + e * Mathf.Sin(oldE)) / (1 - e * Mathf.Cos(oldE));

            i++;
        } while (Mathf.Abs(newE - oldE) > tol || i > 10);

        return newE;
    }

    private float ni2E(float ni)
    {
        return 2 * Mathf.Atan(Mathf.Sqrt((1 - e) / (1 + e)) * Mathf.Atan(ni / 2)); ;
    }

    private float E2ni(float E)
    {
        return 2 * Mathf.Atan(Mathf.Sqrt((1 + e) / (1 - e)) * Mathf.Tan(E / 2));
    }

    private void UpdateOrbitTray()
    {
        int res = 100;

        lineRenderer.positionCount = res+1;
        lineRenderer.SetPositions(CreateOrbitPosArray(res));
    }

    // TODO start it and finish it at current Spacecraft ni
    private Vector3[] CreateOrbitPosArray(int res)
    {
        Vector3[] orbitPosArray = new Vector3[res+1];
        float ni;

        for (int i = 0; i <= res; i++)
        {
            ni = i * 2*Mathf.PI / res;
            orbitPosArray[i] = new Vector3(
                (((p) / (1 + e * Mathf.Cos(ni))) * Mathf.Cos(ni) + e*a) * km2units,
                ((p) / (1 + e * Mathf.Cos(ni))) * Mathf.Sin(ni) * km2units,
                0);
        }

        return orbitPosArray;
    }

    public void UpdateSmaSlider(float value)
    {
        a = value;
        UpdateOrbitalParams();
    }

    public void UpdateEccentricitySlider(float value)
    {
        e = value;
        UpdateOrbitalParams();
    }

    public void UpdateOrbitalParams()
    {

    }
}
