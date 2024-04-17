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
