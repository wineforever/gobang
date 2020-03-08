from net import policy_test
import numpy as np
from keras.models import load_model
policy_model = load_model('policy_model.h5')

from load_data import load_from_sheet
board_state = load_from_sheet('board_state.wf')
#读取数据
state_1 = board_state['Board State Current Self']
state_2 = board_state['Board State Current Enemy']
state_3 = board_state['Board State Enemy Recent Move']
state_4 = board_state['Board State Player']
#将String格式数据转换成Int
state_1 =  [int(x) for x in state_1]
state_2 =  [int(x) for x in state_2]
state_3 =  [int(x) for x in state_3]
state_4 =  [int(x) for x in state_4]
#将List转换成19x19的二维数组
state_1 = np.array(state_1).reshape(19,19)
state_2 = np.array(state_2).reshape(19,19)
state_3 = np.array(state_3).reshape(19,19)
state_4 = np.array(state_4).reshape(19,19)
#将上述List添加到state中，并变形为4x19x19的三维数组
state = []
state.append(state_1)
state.append(state_2)
state.append(state_3)
state.append(state_4)
state = np.array(state).reshape(4,19,19)
#将state添加到state_input中
state_input = []
state_input.append(state)
state_input = np.array(state_input).reshape(-1,4,19,19)
policy = policy_test(state_input,policy_model)

print('Predict done!')