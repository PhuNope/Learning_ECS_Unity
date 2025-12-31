using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace HelloCube.FixedTimestep {
    public class SliderHandler : MonoBehaviour {
        public TextMeshProUGUI SliderValueText;

        public void OnSliderChange() {
            float fixedFps = GetComponent<Slider>().value;

            //! Truy cập World.DefaultGameObjectInjectionWorld là 1 kiểu lập trình không phù hợp trong các dự án phức tạp
            //! Tương tác giữa GameObject và ECS nói chung nên theo chiều ngược lại: thay vì để GameObjects truy cập dữ liệu và mã của ECS, ECS systems nên truy cập GameObject.

            var fixedSimulationGroup = World.DefaultGameObjectInjectionWorld?.GetExistingSystemManaged<FixedStepSimulationSystemGroup>();
            if (fixedSimulationGroup != null) {
                // the group timestep can be set at runtime
                fixedSimulationGroup.Timestep = 1.0f;

                // The current timestep can also be retrieved
                SliderValueText.text = $"{(int)(1.0f / fixedSimulationGroup.Timestep)} updates/sec";
            }
        }
    }
}