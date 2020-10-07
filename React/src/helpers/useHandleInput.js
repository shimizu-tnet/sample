import { useDispatch } from 'react-redux';
import { useMemo } from 'react';

const inputValueChanged = callback => key => e => {
    callback(key, e.target.value);
}

const inputCheckedChanged = callback => key => e => {
    callback(key, e.target.checked);
}

export const useHandleInput = (action, name, current) => {
    const dispatch = useDispatch();
    const callback = useMemo(
        () => {
            return (key, value) => {
                const target = { ...current };
                target[key] = value;
                const payload = {};
                payload[name] = target;
                dispatch(action(payload));
            }
        }, [dispatch, current, action, name]
    );

    return {
        inputValueChanged: inputValueChanged(callback),
        inputCheckedChanged: inputCheckedChanged(callback),
    }
}
