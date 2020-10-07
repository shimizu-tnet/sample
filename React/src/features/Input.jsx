import React, { useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import PageType from '../consts/PageType';
import PageTitle from '../consts/PageTitle';
import Navigation from './Navigation';
import Header from './Header';
import Footer from './Footer';
import ErrorDialog from './Errors/ErrorDialog';
import ReportCover from './ReportCover';
import InheritanceDiagram from './InitialReports/InheritanceDiagram/InheritanceDiagram';
import PropertyDetails from './InitialReports/PropertyDetails/PropertyDetails';
import LandEvaluations from './InitialReports/EvaluationDetails/LandEvaluations';
import HouseEvaluations from './InitialReports/EvaluationDetails/HouseEvaluations';
import EstimatedInheritanceTax from './InitialReports/EstimatedInheritanceTax/EstimatedInheritanceTax';
import PropertyEvaluationList from './InitialReports/PropertyEvaluation/PropertyEvaluationList';
import TaxPaymentList from './InitialReports/TaxPayment/TaxPaymentList';
import HeritageDivisionDiscussionList from './InitialReports/HeritageDivisionDiscussion/HeritageDivisionDiscussionList';
import TaxInvestigationMeasuresSelection from './InitialReports/TaxInvestigationMeasures/TaxInvestigationMeasuresSelection';
import RealEstateConsulting from './InitialReports/RealEstateConsulting/RealEstateConsulting';
import HelpSchedule from './InitialReports/HelpSchedule/HelpSchedule';
import Rationales from './InitialReports/Appendix/Rationales';
import BusinessDescription from './EstimatesReports/BusinessDescription/BusinessDescription';
import Fee from './EstimatesReports/Fee/Fee';
import InheritanceRegistration from './EstimatesReports/InheritanceRegistration/InheritanceRegistration';
import SupplementaryMaterial from './EstimatesReports/SupplementaryMaterial/SupplementaryMaterial';
import { getPdf } from '../api/legacyDocument';
import { setApiFailure, setError, hasResponseError } from './Errors/ErrorSlice';
import { pdfParamaToJson } from '../app/pdfParamSelector';

const Input = () => {
  const paramValues = useSelector(pdfParamaToJson);
  const { accessToken } = useSelector(state => state.login);
  const dispatch = useDispatch();
  const previewPdf = async (type) => {
    try {
      const pdf = await getPdf(type, paramValues, accessToken);
      const url = URL.createObjectURL(pdf);
      window.open(url);
    } catch (err) {
      console.error(err);
      if (hasResponseError(err)) {
        dispatch(setApiFailure({ response: err.response }));
      } else {
        dispatch(setError({ errorMessages: ['処理でエラーが発生したため、再度実行してください。'] }));
      }
    }
  }

  const initalPage = PageType.InitialReportCover;
  const [pageType, setPageType] = useState(initalPage);
  const [pageTitle, setPageTitle] = useState(PageTitle[initalPage]);
  const handleSelectPage = pageType => () => {
    setPageType(pageType)
    setPageTitle(PageTitle[pageType]);
  };
  const selectElement = () => {
    if (pageType === PageType.InitialReportCover || pageType === PageType.EstimatesReportCover) {
      return (<ReportCover />)
    }
    else if (pageType === PageType.InheritanceDiagram) {
      return (<InheritanceDiagram />)
    }
    else if (pageType === PageType.LandEvaluations) {
      return (<LandEvaluations />)
    }
    else if (pageType === PageType.HouseEvaluations) {
      return (<HouseEvaluations />)
    }
    else if (pageType === PageType.PropertyDetails) {
      return (<PropertyDetails />)
    }
    else if (pageType === PageType.EstimatedInheritanceTax) {
      return (<EstimatedInheritanceTax />)
    }
    else if (pageType === PageType.PropertyEvaluation) {
      return (<PropertyEvaluationList />)
    }
    else if (pageType === PageType.TaxPayment) {
      return (<TaxPaymentList />)
    }
    else if (pageType === PageType.HeritageDivisionDiscussion) {
      return (<HeritageDivisionDiscussionList />)
    }
    else if (pageType === PageType.TaxInvestigationMeasures) {
      return (<TaxInvestigationMeasuresSelection />)
    }
    else if (pageType === PageType.RealEstateConsulting) {
      return (<RealEstateConsulting />)
    }
    else if (pageType === PageType.HelpSchedule) {
      return (<HelpSchedule />)
    }
    else if (pageType === PageType.Rationales) {
      return (<Rationales />)
    }
    else if (pageType === PageType.BusinessDescription) {
      return (<BusinessDescription />)
    }
    else if (pageType === PageType.Fee) {
      return (<Fee />)
    }
    else if (pageType === PageType.InheritanceRegistration) {
      return (<InheritanceRegistration />)
    }
    else if (pageType === PageType.SupplementaryMaterial) {
      return (<SupplementaryMaterial />)
    }
    else { return <h2>帳票が選択されていません</h2> }
  }

  if (accessToken === '') {
    return (
      <div className="lost-token">
        <div className="lost-token-inner">
          <div>ログインしてください</div>
          <div>
            <button type="button" className="header-nav-link" onClick={window.close}>閉じる</button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="container">
      <Header pageTitle={pageTitle} />
      <Navigation handleSelectPage={handleSelectPage} previewPdf={previewPdf} />
      <main>
        <div>{selectElement()}</div>
      </main>
      <Footer handleSelectPage={handleSelectPage} pageType={pageType} />
      <ErrorDialog />
    </div>
  );
}

export default Input;