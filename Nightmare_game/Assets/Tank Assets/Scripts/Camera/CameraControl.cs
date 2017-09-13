using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 
    public float m_ScreenEdgeBuffer = 4f;           
    public float m_MinSize = 6.5f;                  
    [HideInInspector] public Transform[] m_Targets; 


    private Camera m_Camera;                        
    private float m_ZoomSpeed;                      
    private Vector3 m_MoveVelocity;                 
    private Vector3 m_DesiredPosition;              


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    private void FindAveragePosition()
    {
		Vector3 averagePos = new Vector3 ();
		int count = 0;
		for (int i = 0; i < m_Targets.Length; i += 1){
			if (m_Targets [i].gameObject.activeSelf) {
				averagePos += m_Targets [i].position;
				count += 1;
			
			}
		}
		if (count > 0) {
			averagePos /= count;
		}
		averagePos.y = transform.position.y;

		m_DesiredPosition = averagePos;
			
	}


    private void Zoom()
    {
		float reqSize = FindRequiredSize ();
		m_Camera.orthographicSize = Mathf.SmoothDamp (m_Camera.orthographicSize, reqSize, ref m_ZoomSpeed, m_DampTime);


	}


    private float FindRequiredSize()
    {
		Vector3 desiredLocalPos = transform.InverseTransformPoint (m_DesiredPosition);

		float size = 0f;

		for (int i = 0; i < m_Targets.Length; i += 1) {
			if (m_Targets [i].gameObject.activeSelf) {
				Vector3 localpos = transform.InverseTransformPoint (m_Targets [i].position);
				Vector3 travel = desiredLocalPos - localpos;

				size = Mathf.Max (size, Mathf.Abs (travel.y));
				size = Mathf.Max (size, Mathf.Abs (travel.x) / m_Camera.aspect);
			}
		}

		size += m_ScreenEdgeBuffer;
		size = Mathf.Max (size, m_MinSize);

		return size;
    }


    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }
}