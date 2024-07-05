# Penalty 

A Goalkeeper RL agent developed using Unity's ML-agents framework

## Setup
```bash
pip install mlagents
```

## Training steps
```bash
mlagents-learn Penalty/Assets/config/configuration.yaml --run-id=Penalty
# Make copies of Arena prefab on scene to train parallely
```

### Visualizing Training Progress with TensorBoard
```bash
tensorboard --logdir=Penalty/results
```
## Demo
[![Watch the video](https://raw.githubusercontent.com/naruto-raj/Penalty/main/thumbnail.jpg)](https://raw.githubusercontent.com/naruto-raj/Penalty/main/Demo.mp4)

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
