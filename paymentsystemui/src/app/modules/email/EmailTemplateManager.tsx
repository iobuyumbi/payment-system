import { useState, useEffect } from 'react';
import axios from 'axios';
import { Content } from '../../../_metronic/layout/components/content';
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { KTCard, KTCardBody } from '../../../_metronic/helpers';
import EmailTemplateForm from './EmailTemplateForm';
import config from '../../../environments/config';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';
import InfoPanel from '../../../_shared/InfoPanel/Index';

const breadCrumbs: Array<PageLink> = [
    {
        title: 'Dashboard',
        path: '/dashboard',
        isSeparator: false,
        isActive: false,
    },
    {
        title: '',
        path: '',
        isSeparator: true,
        isActive: true,
    },
    {
        title: 'Email Templates',
        path: '/email/templates',
        isSeparator: false,
        isActive: true,
    },

    {
        title: '',
        path: '',
        isSeparator: true,
        isActive: true,
    },

]

function EmailTemplateManager() {

    const [templates, setTemplates] = useState<any[]>([]);
    const [selectedTemplate, setSelectedTemplate] = useState<any>(null);
    const [variables, setVariables] = useState<any>({});

    useEffect(() => {
        axios.get('/api/email-templates')
            .then(response => {
                setTemplates(response.data);
            })
            .catch(error => {
                console.error(error);
            });
    }, []);

    const handleSelectTemplate = (template: any) => {
        setSelectedTemplate(template);
        setVariables({});
    };

    const handleUpdateVariable = (name: any, value: any) => {
        setVariables({ ...variables, [name]: value });
    };

    const handleRenderTemplate = () => {
        if (!selectedTemplate) return;
        var renderedBody = selectedTemplate.body;
        Object.keys(variables).forEach((key) => {
            renderedBody = renderedBody.replace(`{${key}}`, variables[key]);
        });
        return renderedBody;
    };

    const InfoPanelContent = (props: any) => {
        return <><p className="info-description">
            Email templates are reusable templates that can be used to send emails to users, contacts, or other stakeholders.
        </p>
            <ul className="info-tips">
                <li>All fields marked with an asterisk (<span className="required"></span> ) are required.</li>
                <li> Curly braces <strong className="fw-bold fs-4"> {`{ }`} </strong> indicate special variables that the system needs in order to insert dynamic data into the email body.
                    Please refrain from changing or removing them as this may cause emails to malfunction.
                </li>
                <li>Use clear and concise subject lines that accurately reflect the content of the email.</li>
                <li>Customize your email templates to fit your brand's voice and style.</li>
            </ul></>;
    };

    useEffect(() => {
        document.title = `Edit Email Template - SDD`;
    }, []);

    return (
        <Content>
            <PageTitleWrapper />
            <PageTitle breadcrumbs={breadCrumbs}>Edit Email Template</PageTitle>
            <div className="my-5">
                <InfoPanel title="Information" ContentComponent={InfoPanelContent} />
            </div>

            <KTCard>
                <KTCardBody>
                    <EmailTemplateForm />
                    <ul>
                        {templates.map((template) => (
                            <li key={template.id}>
                                <a href="#" onClick={() => handleSelectTemplate(template)}>{template.name}</a>
                            </li>
                        ))}
                    </ul>
                    {selectedTemplate && (
                        <div>
                            <h2>{selectedTemplate.name}</h2>
                            <p>Subject: {selectedTemplate.subject}</p>
                            <p>Body:</p>
                            <textarea value={selectedTemplate.body} readOnly />
                            <h3>Variables:</h3>
                            <ul>
                                {selectedTemplate.variables.map((variable: any) => (
                                    <li key={variable.name}>
                                        <label>
                                            {variable.name}:
                                            <input type="text" value={variables[variable.name]} onChange={(e) => handleUpdateVariable(variable.name, e.target.value)} />
                                        </label>
                                    </li>
                                ))}
                            </ul>
                            <p>Rendered Body:</p>
                            <textarea value={handleRenderTemplate()} readOnly />
                        </div>
                    )}
                </KTCardBody>
            </KTCard>

        </Content>
    );
}

export default EmailTemplateManager;