import React from 'react';
import PageType from '../consts/PageType';

const Footer = ({ handleSelectPage, pageType }) => {
    const pageTypes = Object.values(PageType);
    const currentIndex = pageTypes.indexOf(pageType);
    const previousIndex = (currentIndex - 1 >= 0) ? currentIndex - 1 : pageTypes.length - 1;
    const nextIndex = (currentIndex + 1 < pageTypes.length) ? currentIndex + 1 : 0;
    const previousPageType = pageTypes[previousIndex];
    const nextPageType = pageTypes[nextIndex];

    return (
        <footer>
            <div>
                <button className="btn" id="footer-R" onClick={handleSelectPage(previousPageType)}>前のページへ</button>
                <button className="btn" id="footer-L" onClick={handleSelectPage(nextPageType)}>次のページへ</button>
            </div>
        </footer>
    );
}

export default Footer;