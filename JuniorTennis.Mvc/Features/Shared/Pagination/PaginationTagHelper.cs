using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace JuniorTennis.Mvc.Features.Shared.Pagination
{
    public class PaginationTagHelper : TagHelper
    {
        /// <summary>
        /// Paginationタグを構築します。
        /// </summary>
        private TagBuilder paginationTag;

        /// <summary>
        /// １ページに分けた一覧を取得または設定します。
        /// </summary>
        public IPagedList List { get; set; }

        /// <summary>
        /// アクション名を取得または設定します。
        /// </summary>
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        /// <summary>
        /// クリック時のコールバックを取得または設定します。
        /// </summary>
        /// <remarks>引数にページインデックスを渡します。</remarks>
        public string Click { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Rendering.ViewContext"/> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// ITagHelper implementation targeting <input> elements with an asp-for attribute.
        /// </summary>
        protected IHtmlGenerator Generator { get; }

        public PaginationTagHelper(IHtmlGenerator generator)
        {
            this.Generator = generator;
            this.paginationTag = new TagBuilder("div");
            this.paginationTag.AddCssClass("pagination");
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (this.Click == null && this.Action == null)
            {
                throw new InvalidOperationException("clickまたはactionを指定してください。");
            }

            output.TagMode = TagMode.StartTagAndEndTag;
            this.AppendTopButton();
            this.AppendPreviousButton();
            this.AppendPageButtons();
            this.AppendNextButton();
            this.AppendLastButton();
            output.Content.SetHtmlContent(this.paginationTag);
        }

        private void AppendTopButton()
        {
            var topButton = this.GeneratePageButton("<<", 0);
            this.paginationTag.InnerHtml.AppendHtml(topButton);
        }

        private void AppendPreviousButton()
        {
            if (!this.List.HasPreviousPage)
            {
                this.paginationTag.InnerHtml.AppendHtml(new TagBuilder("span"));
                return;
            }

            var previousButton = this.GeneratePageButton("<", this.List.PageIndex - 1);
            this.paginationTag.InnerHtml.AppendHtml(previousButton);
        }

        private void AppendLastButton()
        {
            var lastButton = this.GeneratePageButton(">>", this.List.TotalPageCount - 1);
            this.paginationTag.InnerHtml.AppendHtml(lastButton);
        }

        private void AppendNextButton()
        {
            if (!this.List.HasNextPage)
            {
                this.paginationTag.InnerHtml.AppendHtml(new TagBuilder("span"));
                return;
            }

            var nextButton = this.GeneratePageButton(">", this.List.PageIndex + 1);
            this.paginationTag.InnerHtml.AppendHtml(nextButton);
        }

        private void AppendPageButtons()
        {
            foreach (var pageNumber in this.List.PageNumbers)
            {
                var pageButton = this.GeneratePageButton($"{pageNumber}", pageNumber - 1);
                if (this.List.SelectedPageNumber == pageNumber)
                {
                    pageButton.AddCssClass("selected");
                }

                this.paginationTag.InnerHtml.AppendHtml(pageButton);
            }
        }

        private TagBuilder GeneratePageButton(string linkText, int pageIndex)
        {
            var span = new TagBuilder("span");
            span.AddCssClass("page");
            if (this.Click != null)
            {
                var button = this.GenerateButton(linkText, pageIndex);
                span.InnerHtml.AppendHtml(button);
            }
            else if (this.Action != null)
            {
                var anchor = this.GenerateActionLink(linkText, pageIndex);
                span.InnerHtml.AppendHtml(anchor);
            }

            return span;
        }

        /// <summary>
        /// ボタンを生成します。
        /// </summary>
        /// <param name="linkText">リンクテキスト。</param>
        /// <param name="pageIndex">ページインデックス。</param>
        /// <returns>リンクボタン</returns>
        private TagBuilder GenerateButton(string linkText, int pageIndex)
        {
            var button = new TagBuilder("button");
            button.InnerHtml.AppendHtml(linkText);
            var clickMethod = this.Click.Replace("()", $"({pageIndex})");
            button.Attributes.Add("onclick", clickMethod);
            return button;
        }

        /// <summary>
        /// リンクを生成します。
        /// </summary>
        /// <param name="linkText">リンクテキスト。</param>
        /// <param name="pageIndex">ページインデックス。</param>
        /// <returns>リンクボタン</returns>
        private TagBuilder GenerateActionLink(string linkText, int pageIndex)
        {
            if (this.Action == null)
            {
                throw new InvalidOperationException("Actionが指定されていません。");
            }

            return this.Generator.GenerateActionLink(
                this.ViewContext,
                linkText: linkText,
                actionName: this.Action,
                controllerName: null,
                fragment: null,
                hostname: null,
                htmlAttributes: null,
                protocol: null,
                routeValues: new { page = pageIndex });
        }
    }
}
