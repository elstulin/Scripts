using System;
using UnityEngine;
using System.Collections.Generic;
using HierarchyDict = System.Collections.Generic.Dictionary<string, UnityEngine.Transform>;
using BoneTransformDict = System.Collections.Generic.Dictionary<string, utils.Tuple<UnityEngine.Transform, string>>;

namespace utils
{
	public class MeshCombiner
	{
		#region Operations
		//! Combine mesh.
		/*!
			\return combined mesh instance.
		*/
		public static Mesh Combine(GameObject Object, GameObject NewObject)
		{
			// Dummy parent holder
			GameObject dummy_parent = new GameObject("DummyParent");

			// Be sure new object is at same position as reference object, otherwise
			// combined mesh would be distored
			NewObject.transform.position = Object.transform.position;

			// All skin renderers in all children
			var skin_renderers = Object.GetComponentsInChildren<SkinnedMeshRenderer>();
			// All available bones
			var all_bones = new BoneTransformDict();
			// Traverse through all skinned mesh renderers
			foreach (var renderer in skin_renderers)
			{
				var renderer_bones = renderer.bones;
				foreach (var bone in renderer_bones)
				{
					// Bone doesn't exist, add it
					if (!all_bones.ContainsKey(bone.name))
						all_bones[bone.name] = new utils.Tuple<Transform, string>(bone, bone.parent.name);
				}
			}

			var combineInstanceArrays = new Dictionary<Material, List<CombineInstance>>();
			var bone_weights = new Dictionary<Mesh, BoneWeight[]>();
			// Map between bone name and index
			var added_bones = new Dictionary<string, int>();
			// List of child objects holding the skinned mesh renderers to be
			// destroyed when finished
			var child_objects_to_destroy = new List<GameObject>();

			int bone_index = 0;
			foreach (var renderer in skin_renderers)
			{
				child_objects_to_destroy.Add(renderer.transform.parent.gameObject);

				var renderer_bones = renderer.bones;
				// Add all bones as first and save the indices of them
				foreach (var bone in renderer_bones)
				{
					// Bone not yet added
					if (!added_bones.ContainsKey(bone.name))
						added_bones[bone.name] = bone_index++;
				}
				// Adjust bone weights indices based on real indices of bones
				var bone_weights_list = new BoneWeight[renderer.sharedMesh.boneWeights.Length];
				var renderer_bone_weights = renderer.sharedMesh.boneWeights;
				for (int i = 0; i < renderer_bone_weights.Length; ++i)
				{

					BoneWeight current_bone_weight = renderer_bone_weights[i];

					current_bone_weight.boneIndex0 = added_bones[renderer_bones[current_bone_weight.boneIndex0].name];
					current_bone_weight.boneIndex2 = added_bones[renderer_bones[current_bone_weight.boneIndex2].name];
					current_bone_weight.boneIndex3 = added_bones[renderer_bones[current_bone_weight.boneIndex3].name];
					current_bone_weight.boneIndex1 = added_bones[renderer_bones[current_bone_weight.boneIndex1].name];

					bone_weights_list[i] = current_bone_weight;
				}
				bone_weights[renderer.sharedMesh] = bone_weights_list;

				// Handle bad input
				if (renderer.sharedMaterials.Length != renderer.sharedMesh.subMeshCount)
				{
					Debug.LogError("Mismatch between material count and submesh count. Is this the correct MeshRenderer?");
					continue;
				}

				// Prepare stuff for mesh combination with same materials
				for (int i = 0; i < renderer.sharedMesh.subMeshCount; i++)
				{
					// Material not in dict, add it
					if (!combineInstanceArrays.ContainsKey(renderer.sharedMaterials[i]))
						combineInstanceArrays[renderer.sharedMaterials[i]] = new List<CombineInstance>();
					var actual_mat_list = combineInstanceArrays[renderer.sharedMaterials[i]];
					// Add new instance
					var combine_instance = new CombineInstance();
					combine_instance.transform = renderer.transform.localToWorldMatrix;
					combine_instance.subMeshIndex = i;
					combine_instance.mesh = renderer.sharedMesh;

					actual_mat_list.Add(combine_instance);
				}
				// No need to use it anymore
				renderer.enabled = false;
			}
			var bones_hierarchy = new HierarchyDict();
			// Recreate bone structure
			foreach (var bone in all_bones)
			{
				// Bone not processed, process it
				if (!bones_hierarchy.ContainsKey(bone.Key))
					AddParent(bone.Key, bones_hierarchy, all_bones, dummy_parent);
			}

			// Create bone array from preprocessed dict
			var bones = new Transform[added_bones.Count];
			foreach (var bone in added_bones)
				bones[bone.Value] = bones_hierarchy[bone.Key];

			// Get the root bone
			Transform root_bone = bones[0];

			while (root_bone.parent != null)
			{
				// Get parent
				if (bones_hierarchy.ContainsKey(root_bone.parent.name))
					root_bone = root_bone.parent;
				else
					break;
			}


			// Create skinned mesh renderer GO
			GameObject combined_mesh_go = new GameObject("Combined");
			combined_mesh_go.transform.parent = NewObject.transform;
			combined_mesh_go.transform.localPosition = Vector3.zero;

			// Fill bind poses
			var bind_poses = new Matrix4x4[bones.Length];
			for (int i = 0; i < bones.Length; ++i)
				bind_poses[i] = bones[i].worldToLocalMatrix * combined_mesh_go.transform.localToWorldMatrix;

			// Need to move it to new GO
			root_bone.parent = NewObject.transform;

			// Combine meshes into one
			var combined_new_mesh = new Mesh();
			var combined_vertices = new List<Vector3>();
			var combined_uvs = new List<Vector2>();
			var combined_indices = new List<int[]>();
			var combined_bone_weights = new List<BoneWeight>();
			var combined_materials = new Material[combineInstanceArrays.Count];

			var vertex_offset_map = new Dictionary<Mesh, int>();

			int vertex_index_offset = 0;
			int current_material_index = 0;

			foreach (var combine_instance in combineInstanceArrays)
			{
				combined_materials[current_material_index++] = combine_instance.Key;
				var submesh_indices = new List<int>();
				// Process meshes for each material
				foreach (var combine in combine_instance.Value)
				{
					// Update vertex offset for current mesh
					if (!vertex_offset_map.ContainsKey(combine.mesh))
					{
						// Add vertices for mesh
						combined_vertices.AddRange(combine.mesh.vertices);
						// Set uvs
						combined_uvs.AddRange(combine.mesh.uv);
						// Add weights
						combined_bone_weights.AddRange(bone_weights[combine.mesh]);

						vertex_offset_map[combine.mesh] = vertex_index_offset;
						vertex_index_offset += combine.mesh.vertexCount;
					}
					int vertex_current_offset = vertex_offset_map[combine.mesh];

					var indices = combine.mesh.GetTriangles(combine.subMeshIndex);
					// Need to "shift" indices
					for (int k = 0; k < indices.Length; ++k)
						indices[k] += vertex_current_offset;

					submesh_indices.AddRange(indices);
				}
				// Push indices for given submesh
				combined_indices.Add(submesh_indices.ToArray());
			}

			combined_new_mesh.vertices = combined_vertices.ToArray();
			combined_new_mesh.uv = combined_uvs.ToArray();
			combined_new_mesh.boneWeights = combined_bone_weights.ToArray();

			combined_new_mesh.subMeshCount = combined_materials.Length;
			for (int i = 0; i < combined_indices.Count; ++i)
				combined_new_mesh.SetTriangles(combined_indices[i], i);

			// Create mesh renderer
			SkinnedMeshRenderer combined_skin_mesh_renderer = combined_mesh_go.AddComponent<SkinnedMeshRenderer>();
			combined_skin_mesh_renderer.sharedMesh = combined_new_mesh;
			combined_skin_mesh_renderer.bones = bones;
			combined_skin_mesh_renderer.rootBone = root_bone;
			combined_skin_mesh_renderer.sharedMesh.bindposes = bind_poses;

			combined_skin_mesh_renderer.sharedMesh.RecalculateNormals();
			combined_skin_mesh_renderer.sharedMesh.RecalculateBounds();
			combined_skin_mesh_renderer.sharedMaterials = combined_materials;

			// Destroy children
			foreach (var child in child_objects_to_destroy)
				GameObject.DestroyImmediate(child);
			// Destroy dummy parent
			GameObject.DestroyImmediate(dummy_parent);

			return combined_skin_mesh_renderer.sharedMesh;
		}

		static void AddParent(string BoneName, HierarchyDict BoneHierarchy, BoneTransformDict AllBones, GameObject DummyParent)
		{
			Transform actual_bone = null;
			// Must be bone
			if (AllBones.ContainsKey(BoneName))
			{
				var bone_tuple = AllBones[BoneName];
				// Add parent recursively if not added
				if (!BoneHierarchy.ContainsKey(bone_tuple._2))
				{
					AddParent(bone_tuple._2, BoneHierarchy, AllBones, DummyParent);
					// Unparent all children of parents
					Unparent(BoneHierarchy[bone_tuple._2], DummyParent);
				}


				bone_tuple._1.parent = BoneHierarchy[bone_tuple._2];
				actual_bone = bone_tuple._1;
			}

			BoneHierarchy[BoneName] = actual_bone;
		}

		static void Unparent(Transform Parent, GameObject DummyParent)
		{
			if (Parent != null)
			{
				var unparent_list = new List<Transform>();

				foreach (Transform child in Parent.transform)
					unparent_list.Add(child);

				foreach (var child in unparent_list)
					child.parent = DummyParent.transform;
			}
		}
		#endregion
	}

	internal class Tuple<T1, T2>
	{
		internal string _2;
		internal Transform _1;
		private Transform bone;
		private string name;

		public Tuple(Transform bone, string name)
		{
			this.bone = bone;
			this.name = name;
		}
	}
}