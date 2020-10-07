import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import {
  initializeTaxAccountantWorks,
  initializeAdministrativeScrivenerWorks,
  updateBusinessDescription
} from './BusinessDescriptionSlice';
import TaxAccountantWork from './TaxAccountantWork'
import AdministrativeScrivenerWork from './AdministrativeScrivenerWork'

const BusinessDescription = () => {
  const {
    taxAccountantWorks,
    administrativeScrivenerWorks,
    showOptionWork
  } = useSelector(state => state.businessDescription);

  const dispatch = useDispatch();
  const handleChecked = e => {
    const businessDescription = {
      showOptionWork: e.target.checked
    };
    dispatch(updateBusinessDescription({ businessDescription: businessDescription }));
  }

  const handleInitializeTaxAccountantWorks = () => dispatch(initializeTaxAccountantWorks());
  const handleInitializeAdministrativeScrivenerWorks = () => dispatch(initializeAdministrativeScrivenerWorks());
  const inputTaxAccountantWorks = taxAccountantWorks
    .map(item => <TaxAccountantWork key={item.index} taxAccountantWork={item} />);
  const inputAdministrativeScrivenerWorks = administrativeScrivenerWorks
    .map(item => <AdministrativeScrivenerWork key={item.index} administrativeScrivenerWork={item} />);

  return (
    <>
      <form>
        <label>下記の業務について、税理士法人レガシィにてお手伝いいたします。</label>
        <div className="onecolumn">
          <div>
            <button type="button" className="btn edit-btn" onClick={handleInitializeTaxAccountantWorks}>初期値に戻す</button>
          </div>
          {inputTaxAccountantWorks}
        </div>
        <label>下記の業務について、行政書士法人レガシィにてお手伝いいたします。</label>
        <div className="onecolumn">
          <div>
            <button type="button" className="btn edit-btn" onClick={handleInitializeAdministrativeScrivenerWorks}>初期値に戻す</button>
          </div>
          {inputAdministrativeScrivenerWorks}
        </div>
        <label>別途オプションのご紹介</label>
        <div>
          <input type="checkbox" id="showOptionWork" defaultChecked={showOptionWork} onChange={handleChecked} />
          <label htmlFor="showOptionWork" className="font-wightR">出力する</label>
        </div>
      </form>
    </>
  );
}

export default BusinessDescription;
