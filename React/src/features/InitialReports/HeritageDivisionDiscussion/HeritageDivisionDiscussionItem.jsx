import React from 'react';
import { useHandleInput } from '../../../helpers/useHandleInput';
import { updateHeritageDivisionDiscussion } from './HeritageDivisionDiscussionSlice';

const HeritageDivisionDiscussionItem = props => {
    const { heritageDivisionDiscussion } = props;
    const { inputValueChanged } = useHandleInput(updateHeritageDivisionDiscussion, 'heritageDivisionDiscussion', heritageDivisionDiscussion);

    return (
        <div>
            <textarea value={heritageDivisionDiscussion.text} onChange={inputValueChanged('text')}
                cols="100" rows="5" style={{ resize: 'vertical' }}></textarea>
        </div>
    );
};

export default HeritageDivisionDiscussionItem;