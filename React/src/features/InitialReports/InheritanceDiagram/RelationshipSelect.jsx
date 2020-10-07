import React from 'react';
import Relationship, { getRelationshipName, getRelationshipsByHeirPattern } from './Relationship'

const RelationshipSelect = props => {
    const { relationship, heirPattern } = props;
    const relationships = getRelationshipsByHeirPattern(heirPattern);
    if (relationship === Relationship.Deceased) {
        return (<span>{getRelationshipName(relationship)}</span>);
    }

    return (
        <select value={relationship} onChange={props.handleRelativeValueChanged}>
            {relationships && relationships.map(v => (<option key={v} value={v}>{getRelationshipName(v)}</option>))}
        </select >
    );
}

export default RelationshipSelect;