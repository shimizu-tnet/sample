import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { useHandleInput } from '../helpers/useHandleInput';
import {
    updateNavigation,
    selectAllInitialReport,
    selectAllEstimatesReport,
    isSelectedAny,
} from '../features/NavigationSlice';
import { setError } from '../features/Errors/ErrorSlice';
import PageType from '../consts/PageType';

const Navigation = props => {
    const navigation = useSelector(state => state.navigation);
    const {
        showInitialReportCover,
        showInheritanceDiagram,
        showPropertyDetails,
        showLandEvaluation,
        showHouseEvaluation,
        showEstimatedInheritanceTax,
        showPropertyEvaluation,
        showTaxPayment,
        showHeritageDivisionDiscussion,
        showTaxInvestigationMeasures,
        showInheritanceRealEstateConsulting,
        showHelpSchedule,
        showAppnedix,
        showEstimatesReportCover,
        showBusinessDescription,
        showFee,
        showInheritanceRegistration,
        showSupplementaryMaterial,
    } = navigation;

    const dispatch = useDispatch();
    const { inputCheckedChanged } = useHandleInput(updateNavigation, 'navigation', navigation);
    const handleSelectAllInitialReport = () => dispatch(selectAllInitialReport());
    const handleSelectAllEstimatesReport = () => dispatch(selectAllEstimatesReport());
    const handleSelectPage = props.handleSelectPage;
    const previewPdf = type => async () => {
        if (!isSelectedAny(type, navigation)) {
            dispatch(setError({ errorMessages: ['目次をひとつ以上選択してください。'] }));
            return;
        }
        await props.previewPdf(type);
    }

    const onRotatedWheel = e => document.getElementById("navigationmenu").scrollTop += (e.deltaY * 0.01);

    return (
        <nav onWheel={onRotatedWheel} id="navigationmenu">
            <div className="table-of-contents">
                <h3>初期報告書</h3>
                <button className="btn" onClick={previewPdf("initialReport")}>プレビュー</button>
                <div>
                    <input type="checkbox" name="all" checked="" id="s-all" onChange={handleSelectAllInitialReport} />
                    <label htmlFor="s-all">すべてを選択</label>
                </div>
                <ul>
                    <li>
                        <span id="current" onClick={handleSelectPage(PageType.InitialReportCover)}>表紙</span>
                        <input type="checkbox" id="s1" checked={showInitialReportCover} onChange={inputCheckedChanged('showInitialReportCover')} />
                        <label htmlFor="s1"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.InheritanceDiagram)}>相続関係図</span>
                        <input type="checkbox" id="s2" checked={showInheritanceDiagram} onChange={inputCheckedChanged('showInheritanceDiagram')} />
                        <label htmlFor="s2"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.LandEvaluations)}>土地・借地権の評価明細一覧表</span>
                        <input type="checkbox" id="s4" checked={showLandEvaluation} onChange={inputCheckedChanged('showLandEvaluation')} />
                        <label htmlFor="s4"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.HouseEvaluations)}>家屋の評価明細一覧表</span>
                        <input type="checkbox" id="s5" checked={showHouseEvaluation} onChange={inputCheckedChanged('showHouseEvaluation')} />
                        <label htmlFor="s5"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.PropertyDetails)}>財産明細書</span>
                        <input type="checkbox" id="s3" checked={showPropertyDetails} onChange={inputCheckedChanged('showPropertyDetails')} />
                        <label htmlFor="s3"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.EstimatedInheritanceTax)}>相続税の試算額</span>
                        <input type="checkbox" id="s6" checked={showEstimatedInheritanceTax} onChange={inputCheckedChanged('showEstimatedInheritanceTax')} />
                        <label htmlFor="s6"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.PropertyEvaluation)}>財産の評価について</span>
                        <input type="checkbox" id="s7" checked={showPropertyEvaluation} onChange={inputCheckedChanged('showPropertyEvaluation')} />
                        <label htmlFor="s7"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.TaxPayment)}>納税方法</span>
                        <input type="checkbox" id="s8" checked={showTaxPayment} onChange={inputCheckedChanged('showTaxPayment')} />
                        <label htmlFor="s8"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.HeritageDivisionDiscussion)}>遺産分割協議</span>
                        <input type="checkbox" id="s9" checked={showHeritageDivisionDiscussion} onChange={inputCheckedChanged('showHeritageDivisionDiscussion')} />
                        <label htmlFor="s9"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.TaxInvestigationMeasures)}>税務調査対策</span>
                        <input type="checkbox" id="s10" checked={showTaxInvestigationMeasures} onChange={inputCheckedChanged('showTaxInvestigationMeasures')} />
                        <label htmlFor="s10"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.RealEstateConsulting)}>不動産コンサルティング</span>
                        <input type="checkbox" id="s11" checked={showInheritanceRealEstateConsulting} onChange={inputCheckedChanged('showInheritanceRealEstateConsulting')} />
                        <label htmlFor="s11"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.HelpSchedule)}>お手伝いのスケジュール</span>
                        <input type="checkbox" id="s12" checked={showHelpSchedule} onChange={inputCheckedChanged('showHelpSchedule')} />
                        <label htmlFor="s12"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.Rationales)}>付属資料</span>
                        <input type="checkbox" id="s13" checked={showAppnedix} onChange={inputCheckedChanged('showAppnedix')} />
                        <label htmlFor="s13"></label>
                    </li>
                </ul>
            </div>
            <div className="table-of-contents">
                <h3>お見積書</h3><button className="btn" onClick={previewPdf("estimatesReport")}>プレビュー</button>
                <div>
                    <input type="checkbox" name="all" checked="" id="o-all" onChange={handleSelectAllEstimatesReport} />
                    <label htmlFor="o-all">すべてを選択</label>
                </div>
                <ul>
                    <li>
                        <span onClick={handleSelectPage(PageType.EstimatesReportCover)}>表紙</span>
                        <input type="checkbox" id="m1" checked={showEstimatesReportCover} onChange={inputCheckedChanged('showEstimatesReportCover')} />
                        <label htmlFor="m1"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.BusinessDescription)}>業務内容</span>
                        <input type="checkbox" id="m2" checked={showBusinessDescription} onChange={inputCheckedChanged('showBusinessDescription')} />
                        <label htmlFor="m2"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.Fee)}>報酬</span>
                        <input type="checkbox" id="m3" checked={showFee} onChange={inputCheckedChanged('showFee')} />
                        <label htmlFor="m3"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.InheritanceRegistration)}>相続登記等について</span>
                        <input type="checkbox" id="m4" checked={showInheritanceRegistration} onChange={inputCheckedChanged('showInheritanceRegistration')} />
                        <label htmlFor="m4"></label>
                    </li>
                    <li>
                        <span onClick={handleSelectPage(PageType.SupplementaryMaterial)}>補足資料</span>
                        <input type="checkbox" id="m5" checked={showSupplementaryMaterial} onChange={inputCheckedChanged('showSupplementaryMaterial')} />
                        <label htmlFor="m5"></label>
                    </li>
                </ul>
            </div>
        </nav>
    );
}

export default Navigation;