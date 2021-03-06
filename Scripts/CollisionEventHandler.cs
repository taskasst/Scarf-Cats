using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Obi;

[RequireComponent(typeof(ObiSolver))]
public class CollisionEventHandler : MonoBehaviour {

 	ObiSolver solver;
	
	Obi.ObiSolver.ObiCollisionEventArgs frame;

	void Awake(){
		solver = GetComponent<Obi.ObiSolver>();
	}

	void OnEnable () {
		solver.OnCollision += Solver_OnCollision;
	}

	void OnDisable(){
		solver.OnCollision -= Solver_OnCollision;
	}
	
	void Solver_OnCollision (object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
	{
		frame = e;
        foreach (Oni.Contact contact in e.contacts)
        {
            // this one is an actual collision:
            if (contact.distance < 0.01)
            {
                Component collider;
                if (ObiCollider.idToCollider.TryGetValue(contact.other, out collider))
                {
                    // do something with the collider.
                    
                    // call collision report function
                    PatternPuzzlePointChecker checker = collider.GetComponent<PatternPuzzlePointChecker>();

                    if (checker)
                    {
                        checker.Collide();
                    }
                }
            }
        }
    }

	void OnDrawGizmos()
	{
		if (solver == null || frame == null || frame.contacts == null) return;

		for(int i = 0;  i < frame.contacts.Count; ++i)
		{
			Gizmos.color = (frame.contacts.Data[i].distance < 0.01f) ? Color.red : Color.green;

			Vector3 point = frame.contacts.Data[i].point;
			Vector3 normal = frame.contacts.Data[i].normal;

			Gizmos.DrawSphere(point,0.025f);
	
			Gizmos.DrawRay(point,normal.normalized * 0.1f );//* frame.contacts[i].distance);
		}
	}

}

/*
[RequireComponent(typeof(ObiSolver))]
public class CollisionEventHandler : MonoBehaviour {

 	ObiSolver solver;
	public int counter = 0;
	public Collider targetCollider = null;

	HashSet<int> particles = new HashSet<int>();

	void Awake(){
		solver = GetComponent<Obi.ObiSolver>();
	}

	void OnEnable () {
		solver.OnCollision += Solver_OnCollision;
	}

	void OnDisable(){
		solver.OnCollision -= Solver_OnCollision;
	}
	
	void Solver_OnCollision (object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
	{
		HashSet<int> currentParticles = new HashSet<int>();
		
		for(int i = 0;  i < e.contacts.Count; ++i)
		{
			if (e.contacts.Data[i].distance < 0.001f)
			{

				Component collider;
				if (ObiCollider.idToCollider.TryGetValue(e.contacts.Data[i].other,out collider)){

					if (collider == targetCollider)
						currentParticles.Add(e.contacts.Data[i].particle);

				}
			}
		}

		particles.ExceptWith(currentParticles);
		counter += particles.Count;
		particles = currentParticles;
	}

}
*/

/*[RequireComponent(typeof(ObiSolver))]
public class CollisionEventHandler : MonoBehaviour {

 	ObiSolver solver;
	public Collider targetCollider = null;

	void Awake(){
		solver = GetComponent<Obi.ObiSolver>();
	}

	void OnEnable () {
		solver.OnCollision += Solver_OnCollision;
	}

	void OnDisable(){
		solver.OnCollision -= Solver_OnCollision;
	}
	
	void Solver_OnCollision (object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
	{
		
		for(int i = 0;  i < e.contacts.Count; ++i)
		{
			if (e.contacts.Data[i].distance < 0.001f)
			{
				Component collider;
				if (ObiCollider.idToCollider.TryGetValue(e.contacts.Data[i].other,out collider)){

					if (collider == targetCollider)
						
						solver.viscosities[e.contacts.Data[i].particle] = Mathf.Max(0,solver.viscosities[e.contacts.Data[i].particle] - 0.1f * Time.fixedDeltaTime);
	
				}
			}
		}

	}

}*/
