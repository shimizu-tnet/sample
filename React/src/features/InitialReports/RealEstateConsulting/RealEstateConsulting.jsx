import React from 'react';
import { useSelector } from 'react-redux';
import img_consulting from '../../../images/8-consulting.png';
import { updateNavigation } from '../../NavigationSlice';
import { useHandleInput } from '../../../helpers/useHandleInput';

const RealEstateConsulting = () => {
  const navigation = useSelector(state => state.navigation);
  const { inputCheckedChanged } = useHandleInput(updateNavigation, 'navigation', navigation);
  const imgaeStyle = { opacity: navigation.showInheritanceRealEstateConsulting ? 1 : 0.3 };

  return (
    <>
      <table style={{ borderCollapse: "collapse" }}>
        <tbody>
          <tr>
            <td style={{ border: "1px solid rgb(80, 80, 80)" }}>
              <input type="checkbox" id="showInheritanceRealEstateConsulting"
                checked={navigation.showInheritanceRealEstateConsulting} onChange={inputCheckedChanged('showInheritanceRealEstateConsulting')} />
              <label htmlFor="showInheritanceRealEstateConsulting" style={{ width: 200 }}>当ページを印刷する</label>
            </td>
          </tr>
          <tr>
            <td style={{ width: 450, border: "1px solid rgb(80, 80, 80)" }}>
              <img src={img_consulting} alt="不動産コンサルティング" id="img_hosoku" width="440" style={imgaeStyle} />
            </td>
          </tr>
        </tbody>
      </table>
    </>
  )
}

export default RealEstateConsulting;