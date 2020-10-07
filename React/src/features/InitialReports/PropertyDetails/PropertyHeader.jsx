import React from 'react';

const PropertyHeader = ({ headerValues }) => {
  return (
    <div className="property-table-header">
      <div className="property-1">
        <p>{headerValues[0]}</p>
      </div>
      <div className="property-2">
        <p>{headerValues[1]}</p>
      </div>
      <div className="property-3">
        <p>{headerValues[2]}</p>
      </div>
      <div className="property-4">
        <p>{headerValues[3]}</p>
      </div>
      <div className="property-5">
        <p>{headerValues[4]}</p>
      </div>
    </div>
  );
}

export default PropertyHeader;