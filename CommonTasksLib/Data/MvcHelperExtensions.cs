using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace CommonTasksLib.Data
{
    public static class MvcHelperExtensions
    {

        /// <summary>
        /// Método extensión utilizado para crear un arreglo de objetos del tipo
        /// SelectListItem utilizado para construir instancias de objetos html 
        /// utilizando la sintaxis de razor que requieren un arreglo de objetos
        /// de este tipo tales como DropDownList, ListBox, etc.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto del cual generar el arreglo.</typeparam>
        /// <param name="items">Colección de objetos utilizados para generar el arreglo.</param>
        /// <param name="nameSelector">
        /// Función lambda utilizada para establecer el texto a mostrar por el objeto html
        /// cuando dicho item sea seleccionado.
        /// </param>
        /// <param name="valueSelector">
        /// Función lambda utilizada para establecer el valor interno que representará al
        /// item seleccionado del objeto html generado.
        /// </param>
        /// <param name="selected">
        /// Función lambda utilizada para establecer cual(es) objeto(s) está(n) seleccionado(s)
        /// por defecto en el objeto html generado utilizando el arreglo generado.
        /// </param>
        /// <returns>
        /// Un arreglo del tipo SelecListItem para ser usado para la generación de objetos html.
        /// </returns>
        public static IEnumerable<SelectListItem> ToSelectListItems<T>
            (
             this IEnumerable<T> items,
             Func<T, string> nameSelector,
             Func<T, string> valueSelector,
             Func<T, bool> selected
            )
        {
            return items.OrderBy(item => nameSelector(item))
                   .Select(item =>
                           new SelectListItem
                           {
                               Selected = selected(item),
                               Text = nameSelector(item),
                               Value = valueSelector(item)
                           });
        }

        /// <summary>
        /// Método extensión utilizado para crear un arreglo de objetos del tipo
        /// SelectListItem utilizado para construir instancias de objetos html 
        /// utilizando la sintaxis de razor que requieren un arreglo de objetos
        /// de este tipo tales como DropDownList, ListBox, etc.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto del cual generar el arreglo.</typeparam>
        /// <param name="items">Colección de objetos utilizados para generar el arreglo.</param>
        /// <param name="nameSelector">
        /// Función lambda utilizada para establecer el texto a mostrar por el objeto html
        /// cuando dicho item sea seleccionado.
        /// </param>
        /// <param name="valueSelector">
        /// Función lambda utilizada para establecer el valor interno que representará al
        /// item seleccionado del objeto html generado.
        /// </param>
        /// <returns>
        /// Un arreglo del tipo SelecListItem para ser usado para la generación de objetos html.
        /// </returns>
        public static IEnumerable<SelectListItem> ToSelectListItems<T>
            (
             this IEnumerable<T> items,
             Func<T, string> nameSelector,
             Func<T, string> valueSelector
            )
        {
            return items.OrderBy(item => nameSelector(item))
                   .Select(item =>
                           new SelectListItem
                           {
                               Text = nameSelector(item),
                               Value = valueSelector(item)
                           });
        }

        /// <summary>
        /// Método extensión utilizado para generar una colección de objetos SelectListItem
        /// utilizados para la construcción de dropdown lists.
        /// </summary>
        /// <typeparam name="TEnum">Tipo de datos del objeto Enumerado.</typeparam>
        /// <typeparam name="TAttributeType">Tipo de instancia interna de una opción del enumerado.</typeparam>
        /// <param name="enumObj">Instancia de objeto enumeración para invocar el método.</param>
        /// <param name="useIntegerValue">Valor que especifica si se desea usar el valor int interno del enumerado.</param>
        /// <returns>Colección de tipo SelectListItem con las opciones del enumerado.</returns>
        public static IEnumerable<SelectListItem> EnumToSelectList<TEnum, TAttributeType>(this TEnum enumObj, bool useIntegerValue)
            where TEnum : struct
        {
            Type type = typeof(TEnum);

            var fields = type.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public);

            var values = from field in fields
                         select new SelectListItem
                         {
                             Value = (useIntegerValue) ? field.GetRawConstantValue().ToString() : field.Name,
                             Text = field.GetCustomAttributes(typeof(TAttributeType), true).
                                        FirstOrDefault().ToString() ?? field.Name,
                             Selected =
                                 (useIntegerValue)
                                     ? (Convert.ToInt32(field.GetRawConstantValue()) &
                                        Convert.ToInt32(enumObj)) ==
                                       Convert.ToInt32(field.GetRawConstantValue())
                                     : enumObj.ToString().Contains(field.Name)
                         };


            return values;
        }

        /// <summary>
        /// Html LabelFor helper diseñado para añadir una clase custom
        /// utilizada para los mensajes de error de bootstrap
        /// </summary>
        /// <typeparam name="TModel">Modelo del cual generar</typeparam>
        /// <typeparam name="TValue">Valor del modelo</typeparam>
        /// <param name="html">objeto HtmlHelper referenciado</param>
        /// <param name="expression">Expresion Lambda pra conocer el campo seleccionado</param>
        /// <param name="htmlAttributes">Atributos html a agregar al tag</param>
        /// <returns></returns>
        public static MvcHtmlString CLabelFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = null;
            addCustomClass(htmlAttributes, out attributes, "control-label");

            return html.LabelFor(expression, attributes);
        }

        public static MvcHtmlString CTextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = null;
            addCustomClass(htmlAttributes, out attributes, "form-control");

            return html.TextBoxFor(expression, attributes);
        }

        public static MvcHtmlString CTextAreaFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = null;
            addCustomClass(htmlAttributes, out attributes, "form-control");

            return html.TextAreaFor(expression, attributes);
        }

        public static MvcHtmlString CPasswordFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = null;
            addCustomClass(htmlAttributes, out attributes, "form-control");

            return html.PasswordFor(expression, attributes);
        }

        public static MvcHtmlString CValidationMessageFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression)
        {
            IDictionary<string, object> attributes = null;
            addCustomClass(null, out attributes, "help-block");

            return html.ValidationMessageFor(expression, null, attributes);
        }

        public static MvcHtmlString CTextBox(
            this HtmlHelper html,
            string name,
            object value = null,
           object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = null;
            addCustomClass(htmlAttributes, out attributes, "form-control");

            return html.TextBox(name, value, attributes);
        }

        public static MvcHtmlString CTextArea(
            this HtmlHelper html,
            string name,
           object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = null;
            addCustomClass(htmlAttributes, out attributes, "form-control");

            return html.TextArea(name, attributes);
        }

        public static MvcHtmlString CPassword(
            this HtmlHelper html,
            string name,
            object value = null,
            IDictionary<string, object> htmlAttributes = null)
        {
            IDictionary<string, object> attributes = null;
            addCustomClass(htmlAttributes, out attributes, "form-control");

            return html.Password(name, value, attributes);
        }

        public static SelectList ToSelectList<EnumType>(this EnumType enumObject)
            where EnumType : struct, IComparable, IFormattable, IConvertible
        {
            var values = from EnumType e in Enum.GetValues(typeof(EnumType))
                         select new { Id = e, Name = e.ToString() };
            return new SelectList(values, "Id", "Name", enumObject);
        }

        public static MvcHtmlString CDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            string optionLabel = null,
            object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = null;
            addCustomClass(htmlAttributes, out attributes, "form-control");

            return html.DropDownListFor(expression, selectList, (optionLabel == null) ? string.Empty : optionLabel, attributes);
        }

        public static MvcHtmlString CListBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = null;
            addCustomClass(htmlAttributes, out attributes, "form-control");

            return html.ListBoxFor(expression, selectList, attributes);
        }

        public static MvcHtmlString CDropDownList(
            this HtmlHelper html,
            string name,
            IEnumerable<SelectListItem> selectList,
            string optionLabel = null,
            object htmlAttributes = null)
        {
            IDictionary<string, object> attributes = null;
            addCustomClass(htmlAttributes, out attributes, "form-control");

            return html.DropDownList(name, selectList, (optionLabel == null) ? string.Empty : optionLabel, attributes);
        }

        private static void addCustomClass(
            object htmlAttributes,
            out IDictionary<string, object> attributes,
            string Customclass)
        {
            attributes = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            object cssClass;
            if (attributes.TryGetValue("class", out cssClass) == false)
            {
                cssClass = "";
            }
            attributes["class"] = cssClass + " " + Customclass;
        }

        public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper, string imageClass, string action, object htmlAttributes)
        {
            TagBuilder builder;
            UrlHelper urlHelper;
            urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            builder = new TagBuilder("a");
            builder.InnerHtml = string.Format("<i class=\"glyphicon {0}\">", imageClass);
            builder.Attributes["href"] = urlHelper.Action(action);
            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString ActionLinkImage(this HtmlHelper htmlHelper, string imageClass, string actionName, string title = "", object routeValues = null, object htmlAttributes = null)
        {
            TagBuilder builder;
            UrlHelper urlHelper;
            htmlAttributes = (htmlAttributes == null) ? new Dictionary<string, object>() : htmlAttributes;
            var attributes = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            builder = new TagBuilder("a");
            builder.InnerHtml = string.Format("<i class=\"glyphicon glyphicon-{0} \"></i>", imageClass, title);
            builder.Attributes["href"] = urlHelper.Action(actionName, routeValues);
            attributes.Add("title", title);
            attributes.Add("data-toggle", "tooltip");
            builder.MergeAttributes(new RouteValueDictionary(attributes));

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString ActionLinkImage(this HtmlHelper htmlHelper, string imageClass, string actionName, string controllerName, string title = "", object routeValues = null, object htmlAttributes = null)
        {
            TagBuilder builder;
            UrlHelper urlHelper;
            htmlAttributes = (htmlAttributes == null) ? new Dictionary<string, object>() : htmlAttributes;
            var attributes = (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            builder = new TagBuilder("a");
            builder.InnerHtml = string.Format("<i class=\"glyphicon glyphicon-{0} \"></i>", imageClass, title);
            builder.Attributes["href"] = urlHelper.Action(actionName, controllerName, routeValues);
            attributes.Add("title", title);
            attributes.Add("data-toggle", "tooltip");

            builder.MergeAttributes(new RouteValueDictionary(attributes));

            return MvcHtmlString.Create(builder.ToString());
        }

    }
}
