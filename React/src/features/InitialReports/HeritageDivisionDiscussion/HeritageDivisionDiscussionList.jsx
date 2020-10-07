import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { initializeHeritageDivisionDiscussions } from './HeritageDivisionDiscussionSlice';
import HeritageDivisionDiscussionItem from './HeritageDivisionDiscussionItem';

const HeritageDivisionDiscussionList = () => {
    const { heritageDivisionDiscussions } = useSelector(
        state => state.heritageDivisionDiscussion
    );
    const rows = heritageDivisionDiscussions.map((item, index) =>
        <HeritageDivisionDiscussionItem key={index} heritageDivisionDiscussion={item} />);
    const dispatch = useDispatch();
    const handleInitialize = () => dispatch(initializeHeritageDivisionDiscussions());

    return (
        <>
            <div>
                <button type="button" className="btn edit-btn" onClick={handleInitialize}>初期値に戻す</button>
            </div>
            <div className="onecolumn">
                {rows}
            </div>
        </>
    );
}

export default HeritageDivisionDiscussionList;