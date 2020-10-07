import React from 'react';
import { useSelector } from 'react-redux';
import img_hosoku from '../../../images/5-hosoku.png';
import { updateNavigation } from '../../NavigationSlice';
import { updateDescription } from './SupplementaryMaterialSlice';
import { useHandleInput } from '../../../helpers/useHandleInput';

const SupplementaryMaterial = () => {
  const navigation = useSelector(state => state.navigation);
  const supplementaryMaterial = useSelector(state => state.supplementaryMaterial);
  const { inputCheckedChanged } = useHandleInput(updateNavigation, 'navigation', navigation);
  const { inputValueChanged } = useHandleInput(updateDescription, 'supplementaryMaterial', supplementaryMaterial);
  const imgaeStyle = { opacity: navigation.showSupplementaryMaterial ? 1 : 0.3 };

  return (
    <>
      <table style={{ borderCollapse: "collapse" }}>
        <tbody>
          <tr>
            <td style={{ border: "1px solid rgb(80, 80, 80)" }}>
              <input type="checkbox" id="showSupplementaryMaterial"
                checked={navigation.showSupplementaryMaterial} onChange={inputCheckedChanged('showSupplementaryMaterial')} />
              <label htmlFor="showSupplementaryMaterial" style={{ width: 200 }}>当ページを印刷する</label>
            </td>
          </tr>
          <tr>
            <td style={{ width: 450, border: "1px solid rgb(80, 80, 80)" }}>
              <img src={img_hosoku} alt="補足資料" id="img_hosoku" width="440" style={imgaeStyle} />
            </td>
          </tr>
        </tbody>
      </table>
      <div className="comment-area">
        <div className="title">
          <p>主な別途加算報酬</p>
        </div>
        <div className="comment">
          <input type="text" value={supplementaryMaterial.additionalFeeDescription} onChange={inputValueChanged("additionalFeeDescription")} />
        </div>
      </div>
    </>
  )
}

export default SupplementaryMaterial;