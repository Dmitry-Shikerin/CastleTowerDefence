using System;
using System.Collections.Generic;
using UnityEngine;

namespace TeoGames.Mesh_Combiner.Scripts.BlendShape {
	[Serializable]
	public class BlendShapeContainer {
		public Dictionary<string, Dictionary<int, BlendShapeFrame>> BlendShape =
			new Dictionary<string, Dictionary<int, BlendShapeFrame>>();

		public Dictionary<string, (int, Transform)> LiveSync = new Dictionary<string, (int, Transform)>();

		public int length;

		public void Clear() {
			length = 0;
			BlendShape.Clear();
			LiveSync.Clear();
		}

		public void Extend(BlendShapeContainer container) {
			foreach (var pair in container.BlendShape) {
				if (!BlendShape.TryGetValue(pair.Key, out var existing)) {
					existing = new Dictionary<int, BlendShapeFrame>();
					BlendShape[pair.Key] = existing;
				}

				foreach (var frame in pair.Value) existing[frame.Key + length] = frame.Value;
			}

			foreach (var sync in container.LiveSync) LiveSync[sync.Key] = sync.Value;

			length += container.length;
		}

		public BlendShapeContainer Export(BlendShapeConfiguration conf, Transform renTransform, int id) {
			if (!conf.enabled) return this;

			var res = new BlendShapeContainer() { length = length };
			var syncCnt = conf.liveSync.Count;

			if (conf.merge) {
				res.BlendShape = BlendShape;
				for (var i = 0; i < syncCnt; i++) res.LiveSync[conf.liveSync[i]] = (i, renTransform);
			} else {
				var prefix = $"[{id}] ";
				foreach (var pair in BlendShape) res.BlendShape[prefix + pair.Key] = pair.Value;
				for (var i = 0; i < syncCnt; i++) res.LiveSync[prefix + conf.liveSync[i]] = (i, renTransform);
			}

			return res;
		}
	}
}