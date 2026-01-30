---
title: 'About <subject>'
shortTitle: '<subject>'
intro: 'Article intro. See tips for a great intro below.'
product: "optional product callout"
type: overview
topics:
  - topic
versions:
  - version
---

{% comment %}
- Follow the guidelines in https://docs.github.com/contributing/writing-for-github-docs/content-model#conceptual to write this article.
- Great intros give readers a quick understanding of what's in the article, so they can tell whether it's relevant to them before moving ahead. For more tips, see https://docs.github.com/contributing/writing-for-github-docs/content-model
- For product callout info, see https://github.com/github/docs/tree/main/content#product
- For product version instructions, see https://github.com/github/docs/tree/main/content#versioning
- Remove these comments from your article file when you're done writing.
{% endcomment %}

## Introduction

This page serves as the central location for all Culinary Command related documentation. 

Github uses Ruby + Jekyll to generate and host this page. More specifically, Github looks at whatever is in the `CulinaryCommand/docs/` directory and deploys it.

## How To Contribute

All related information to documentation is located under the `docs/` directory. `index.md` is where all of the content is loaded from, so, if you want to add information, that is the file you want to edit. `_config.yml` serves as a configuration file that Github reads the theme from. 

## Table of Contents

- [Terraform](#terraform)

## Terraform {#terraform}

[Terraform](https://developer.hashicorp.com/terraform) is an Infrastructure as Code (IaC) tool that deploys all AWS related infrastructure. This is automatically done through the CI/CD pipeline.

Currently, the only resource that Terraform deploys is the lightsail instance that the Culinary Commmand app is hosted on.