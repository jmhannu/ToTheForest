using UnityEngine;
public class DriveCar : MonoBehaviour
{
    private float speed;
    private Vector3 direction;

    void Start()
    {
        speed = LevelManager.CurrentSpeed * 2;

        SetDirection();
    }

    void Update()
    {
            transform.Translate(direction * Time.deltaTime * speed, Space.World);

            if (transform.position.z < LevelManager.MinZ)
            {
                if (LevelManager.Last) Destroy(this.gameObject);
                transform.position = new Vector3(transform.position.x, transform.position.y, LevelManager.MaxZ);
            }

            else if (transform.position.z > LevelManager.MaxZ)
            {
                if(LevelManager.Last) Destroy(this.gameObject);
                else transform.position = new Vector3(transform.position.x, transform.position.y, LevelManager.MinZ);
            }
    }

    private void SetDirection()
    {
            float rotationEuler = transform.rotation.eulerAngles.y;

            if (rotationEuler == 0) direction = Vector3.forward;
            else if (rotationEuler == 180) direction = Vector3.back;
    }

    public void SetSpeed(float _multiplicator)
    {
        speed = LevelManager.CurrentSpeed * _multiplicator;
    }
}
