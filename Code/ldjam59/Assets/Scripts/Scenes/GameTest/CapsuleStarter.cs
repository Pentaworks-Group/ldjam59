using UnityEngine;

	public class CapsuleStarter : MonoBehaviour
	{

    private void Update()
    {
        Debug.Log("asdf");
        //gameObject.transform.position = new Vector3(5,5,5);
        if (transform.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.AddForce(new Vector3(1, 1, 1));
        }
    }


}
