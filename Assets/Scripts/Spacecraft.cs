using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Spacecraft : MonoBehaviour
{
    [SerializeField] float e;
    [SerializeField] float a;
    [SerializeField] Transform primaryTransform;
    [SerializeField] Text apoapsisText;
    [SerializeField] Text periapsisText;

    // states
    float trueAnomaly; // in rad
    float[] rVect = { 0, 0 }; // km
    float rPeriapsis;
    float rApoapsis;
    float p;

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
        // update states
        UpdateTrueAnomaly();
        UpdateSpacecraftPos();
        UpdateOrbitTray();

        // update GUI
        UIManagerRef.UpdateApoapsisText(rApoapsis - radiusEarth);
        UIManagerRef.UpdatePeriapsisText(rPeriapsis - radiusEarth);
    }

    private void UpdateSpacecraftPos()
    {
        float rScalar = p / (1 + e * Mathf.Cos(trueAnomaly));

        rVect[0] = rScalar * Mathf.Cos(trueAnomaly);
        rVect[1] = rScalar * Mathf.Sin(trueAnomaly);

        gameObject.transform.position = new Vector2(rVect[0] * km2units, rVect[1] * km2units);
    }

    private void UpdateTrueAnomaly()
    {
        // circular case
        // TODO

        // elliptic case
        if (e > 0 && e < 1)
        {
            float ellipticalEccentricAnomaly = SolveEllipticInverseKeplerProblem();
            trueAnomaly = E2ni(ellipticalEccentricAnomaly);
        }

        // parabolic case
        // TODO

        // hyperbolic case
        // TODO
    }

    // consider deltaT = time from start of everything
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

    // consider deltaT = time of frame
    private float SolveEllipticInverseKeplerProblem2()
    {
        float newE2, oldE2, E1, M;
        float tol = 0.001f;

        E1 = ni2E(trueAnomaly);
        M = Mathf.Sqrt(muEarth / Mathf.Pow(a, 3)) * Time.deltaTime - e * Mathf.Sin(E1) + E1;
        newE2 = M;
        int i = 0;

        do
        {
            oldE2 = newE2;
            newE2 = oldE2 + (M - oldE2 + e * Mathf.Sin(oldE2)) / (1 - e * Mathf.Cos(oldE2));

            i++;
        } while (Mathf.Abs(newE2 - oldE2) > tol || i > 10);

        Debug.Log(i);

        return newE2;
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
                ((p) / (1 + e * Mathf.Cos(ni))) * Mathf.Cos(ni) * km2units,
                ((p) / (1 + e * Mathf.Cos(ni))) * Mathf.Sin(ni) * km2units,
                0);
        }

        return orbitPosArray;
    }
}
