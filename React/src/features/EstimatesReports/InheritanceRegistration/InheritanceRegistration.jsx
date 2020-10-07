import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { initializeInheritanceRegistrations } from './InheritanceRegistrationSlice';
import InheritanceRegistrationItem from './InheritanceRegistrationItem';

const InheritanceRegistration = () => {
  const { inheritanceRegistrations } = useSelector(state => state.inheritanceRegistration);
  const dispatch = useDispatch();
  const handleInitializeInheritanceRegistrations = () => dispatch(initializeInheritanceRegistrations());
  const inputInheritanceRegistrations = inheritanceRegistrations
    .map(item => <InheritanceRegistrationItem key={item.index} inheritanceRegistration={item} />);

  return (
    <>
      <form>
        <div className="onecolumn">
          <div>
            <button type="button" className="btn edit-btn" onClick={handleInitializeInheritanceRegistrations} >初期値に戻す</button>
          </div>
          {inputInheritanceRegistrations}
        </div>
      </form>
    </>
  );
}

export default InheritanceRegistration;
