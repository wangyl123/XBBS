--
-- PostgreSQL database dump
--

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

--
-- Name: seq_category_id; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE seq_category_id
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.seq_category_id OWNER TO postgres;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: categories; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE categories (
    cid smallint DEFAULT nextval('seq_category_id'::regclass) NOT NULL,
    pid smallint NOT NULL,
    cname character varying(30),
    content character varying(255),
    keywords character varying(255),
    ico character varying(128),
    master character varying(100) NOT NULL,
    permit character varying(255),
    listnum integer,
    clevel character varying(25),
    cord smallint
);


ALTER TABLE public.categories OWNER TO postgres;

--
-- Name: COLUMN categories.cname; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN categories.cname IS '分类名称';


--
-- Name: jexus_comments_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE jexus_comments_seq
    START WITH 6
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.jexus_comments_seq OWNER TO postgres;

--
-- Name: comments; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE comments (
    id integer DEFAULT nextval('jexus_comments_seq'::regclass) NOT NULL,
    fid integer NOT NULL,
    uid integer NOT NULL,
    content text,
    replytime timestamp(6) with time zone
);


ALTER TABLE public.comments OWNER TO postgres;

--
-- Name: favorites; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE favorites (
    id integer NOT NULL,
    uid integer NOT NULL,
    favorites integer NOT NULL,
    content text NOT NULL
);


ALTER TABLE public.favorites OWNER TO postgres;

--
-- Name: forums; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE forums (
    fid integer NOT NULL,
    cid smallint NOT NULL,
    uid integer NOT NULL,
    ruid integer,
    title character varying(128),
    keywords character varying(255),
    content text,
    addtime timestamp(6) with time zone,
    updatetime timestamp(6) with time zone,
    lastreply timestamp(6) with time zone,
    views integer,
    comments smallint,
    favorites bigint,
    closecomment smallint,
    is_top smallint NOT NULL,
    is_hidden smallint NOT NULL,
    ord bigint NOT NULL
);


ALTER TABLE public.forums OWNER TO postgres;

--
-- Name: jexus_forums_fid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE jexus_forums_fid_seq
    START WITH 9
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.jexus_forums_fid_seq OWNER TO postgres;

--
-- Name: jexus_forums_fid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE jexus_forums_fid_seq OWNED BY forums.fid;


--
-- Name: users; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE users (
    uid integer NOT NULL,
    username character varying(20),
    password character(32),
    openid character(32) NOT NULL,
    email character varying(50),
    avatar character varying(100),
    homepage character varying(50),
    money integer,
    signature text,
    forums integer,
    replies integer,
    notices smallint,
    follows integer NOT NULL,
    regtime timestamp(6) with time zone,
    lastlogin timestamp(6) with time zone,
    lastpost timestamp(6) with time zone,
    qq character varying(20),
    group_type smallint NOT NULL,
    gid smallint NOT NULL,
    ip character(15),
    location character varying(128),
    token character varying(40),
    introduction text,
    is_active smallint NOT NULL,
    nick_name character varying(20)
);


ALTER TABLE public.users OWNER TO postgres;

--
-- Name: jexus_users_uid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE jexus_users_uid_seq
    START WITH 4
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.jexus_users_uid_seq OWNER TO postgres;

--
-- Name: jexus_users_uid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE jexus_users_uid_seq OWNED BY users.uid;


--
-- Name: seq_links_id; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE seq_links_id
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.seq_links_id OWNER TO postgres;

--
-- Name: links; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE links (
    id smallint DEFAULT nextval('seq_links_id'::regclass) NOT NULL,
    name character varying(100),
    url character varying(200),
    logo character varying(200),
    is_hidden smallint NOT NULL
);


ALTER TABLE public.links OWNER TO postgres;

--
-- Name: notifications; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE notifications (
    nid integer NOT NULL,
    fid integer,
    suid integer,
    nuid integer NOT NULL,
    ntype smallint,
    ntime integer
);


ALTER TABLE public.notifications OWNER TO postgres;

--
-- Name: page; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE page (
    pid smallint NOT NULL,
    title character varying(100),
    content text,
    go_url character varying(100),
    add_time integer,
    is_hidden smallint
);


ALTER TABLE public.page OWNER TO postgres;

--
-- Name: settings; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE settings (
    sid smallint NOT NULL,
    title character varying(255) NOT NULL,
    value text NOT NULL,
    type smallint NOT NULL
);


ALTER TABLE public.settings OWNER TO postgres;

--
-- Name: tags; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE tags (
    tag_id integer NOT NULL,
    tag_title character varying(30) NOT NULL,
    forums integer NOT NULL
);


ALTER TABLE public.tags OWNER TO postgres;

--
-- Name: tags_relation; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE tags_relation (
    tag_id integer NOT NULL,
    fid integer
);


ALTER TABLE public.tags_relation OWNER TO postgres;

--
-- Name: user_follow; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE user_follow (
    follow_id bigint NOT NULL,
    uid bigint NOT NULL,
    follow_uid bigint NOT NULL,
    addtime integer NOT NULL
);


ALTER TABLE public.user_follow OWNER TO postgres;

--
-- Name: user_groups_gid; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE user_groups_gid
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.user_groups_gid OWNER TO postgres;

--
-- Name: user_groups; Type: TABLE; Schema: public; Owner: postgres; Tablespace: 
--

CREATE TABLE user_groups (
    gid integer DEFAULT nextval('user_groups_gid'::regclass) NOT NULL,
    group_type smallint NOT NULL,
    group_name character varying(50),
    usernum integer NOT NULL
);


ALTER TABLE public.user_groups OWNER TO postgres;

--
-- Name: fid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY forums ALTER COLUMN fid SET DEFAULT nextval('jexus_forums_fid_seq'::regclass);


--
-- Name: uid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY users ALTER COLUMN uid SET DEFAULT nextval('jexus_users_uid_seq'::regclass);


--
-- Data for Name: categories; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY categories (cid, pid, cname, content, keywords, ico, master, permit, listnum, clevel, cord) FROM stdin;
1	0	主版块	 	 	\N	 	1	0	\N	\N
2	1	Jexus安装部署	 	 	\N	 	1	1	\N	\N
3	1	Mono@Ubuntu	 	 	\N	 	1	1	\N	\N
4	1	Mono@CentOS	 	 	\N	 	1	1	\N	\N
5	1	树莓派	 	 	\N	 	1	1	\N	\N
6	0	测试根	测试	测试	\N	admin	1,2,3,5	\N	\N	\N
8	0	测试根2	asfsdfa	asfa	\N	adsdaflj,asdfjkl	1,2	0	\N	\N
7	8	测试子	测试子	测试子	\N	admin	\N	0	\N	\N
\.


--
-- Data for Name: comments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY comments (id, fid, uid, content, replytime) FROM stdin;
7	11	4	AABBDD	2014-04-05 16:12:16.345828+08
8	11	4	asdfasdfsdafsadf	2014-04-05 16:12:25.48102+08
9	11	4	sadfasdfsdafsdafsadf	2014-04-05 16:12:28.094079+08
10	11	4	sadfsadf	2014-04-05 16:12:35.803186+08
11	12	4	灰常灰常好	2014-04-05 16:45:10.7679+08
12	13	4	回复一下	2014-04-07 21:28:02.273605+08
\.


--
-- Data for Name: favorites; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY favorites (id, uid, favorites, content) FROM stdin;
\.


--
-- Data for Name: forums; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY forums (fid, cid, uid, ruid, title, keywords, content, addtime, updatetime, lastreply, views, comments, favorites, closecomment, is_top, is_hidden, ord) FROM stdin;
10	3	4	\N	sadf	\N	asfsadf	2014-03-30 20:19:39.607775+08	2014-03-30 20:19:39.608773+08	\N	1	0	\N	\N	0	0	0
12	4	37	4	le-haha	\N	lasjflkjsdalfjklsadjfsadfsdfsadfsdafasdf	2014-04-05 16:42:42.452457+08	2014-04-05 16:42:42.452457+08	2014-04-05 16:45:10.778914+08	14	1	\N	\N	0	0	0
11	3	4	4	AABBCC	\N	432edXB	2014-04-05 13:00:44.117859+08	2014-04-05 16:12:03.3336+08	2014-04-05 16:12:35.813179+08	28	4	\N	\N	1	0	0
13	3	4	4	发表一篇	\N	哇咔咔~~！！！	2014-04-07 21:27:50.442212+08	2014-04-07 21:27:50.442212+08	2014-04-07 21:28:02.287641+08	3	1	\N	\N	0	0	0
\.


--
-- Name: jexus_comments_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('jexus_comments_seq', 12, true);


--
-- Name: jexus_forums_fid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('jexus_forums_fid_seq', 13, true);


--
-- Name: jexus_users_uid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('jexus_users_uid_seq', 37, true);


--
-- Data for Name: links; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY links (id, name, url, logo, is_hidden) FROM stdin;
1	百度	http://www.baidu.com	\N	0
2	Jexus	http://www.linuxdot.net	\N	0
\.


--
-- Data for Name: notifications; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY notifications (nid, fid, suid, nuid, ntype, ntime) FROM stdin;
\.


--
-- Data for Name: page; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY page (pid, title, content, go_url, add_time, is_hidden) FROM stdin;
\.


--
-- Name: seq_category_id; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('seq_category_id', 10, true);


--
-- Name: seq_links_id; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('seq_links_id', 4, true);


--
-- Data for Name: settings; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY settings (sid, title, value, type) FROM stdin;
5	site_run	0	0
9	money_title	银币	0
11	is_rewrite	off	0
14	storage_set	local	0
28	protocol	smtp	0
29	smtp_host	192.168.0.110	0
30	smtp_port	25	0
20	encryption_key	f7020a8ae0152fff2a1189c35efe1b65	0
31	smtp_user	xiaodiejinghong@cnblogs.com	0
32	smtp_pass	xiaodiejinghong	0
1	site_name	Jexus	0
2	welcome_tip	欢迎访问Jexus社区	0
3	short_intro	Mono中文社区	0
7	site_keywords	轻量 ?  易用  ?  社区系统	0
16	logo	小<span class='red'>蝶</span>惊鸿	0
4	show_captcha	off	0
12	show_editor	on	0
15	auto_tag	off	0
17	site_close	on	0
18	site_close_msg	站点关闭\r\n\r\n\r\n\r\n\r\n\r\n\r\n	0
6	site_stats	统计代码\r\n\r\n\r\n\r\n\r\n\r\n\r\n	0
8	site_description	最火的Mono社区	0
19	reward_title	种子	0
13	comment_order	desc	0
22	is_approve	on	0
23	home_page_num	10	0
10	per_page_num	20	0
25	timespan	100	0
21	words_limit	5000	0
26	qq_appid		0
27	qq_appkey		0
\.


--
-- Data for Name: tags; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY tags (tag_id, tag_title, forums) FROM stdin;
\.


--
-- Data for Name: tags_relation; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY tags_relation (tag_id, fid) FROM stdin;
\.


--
-- Data for Name: user_follow; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY user_follow (follow_id, uid, follow_uid, addtime) FROM stdin;
\.


--
-- Data for Name: user_groups; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY user_groups (gid, group_type, group_name, usernum) FROM stdin;
1	1	管理员	1
2	2	版主	2
3	3	普通会员	-2
5	3	5	0
\.


--
-- Name: user_groups_gid; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('user_groups_gid', 10, true);


--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY users (uid, username, password, openid, email, avatar, homepage, money, signature, forums, replies, notices, follows, regtime, lastlogin, lastpost, qq, group_type, gid, ip, location, token, introduction, is_active, nick_name) FROM stdin;
27	v	9E3669D19B675BD57058FD4664205D2A	                                	v@v.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:55:13.445492+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
5	lele	69BFC4EF467B367E3515CDCF693E65DB	                                	lele@lele.com	\N	lele.com.cn	\N	小蝶惊鸿	\N	\N	\N	0	2014-03-07 23:12:58.347398+08	\N	\N	55555	2	3	::1            	广东中山	\N	帅气、有型	1	\N
10	b	92EB5FFEE6AE2FEC3AD71C777531578F	                                	b@b.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:36:12.579374+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
11	c	4A8A08F09D37B73795649038408B5F33	                                	c@c.om	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:36:26.713475+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
12	e	E1671797C52E15F763380B45E841EC32	                                	e@e.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:37:16.236373+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
13	f	8FA14CDD754F91CC6554C9E71929CCE7	                                	f@f.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:37:28.314862+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
14	g	B2F5FF47436671B6E533D8DC3614845D	                                	g@g.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:37:44.820853+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
15	io	865C0C0B4AB0E063E5CAA3387C1A8741	                                	i@i.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:38:14.836405+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
17	l	2DB95E8E1A9267B7A1188556B2013B33	                                	l@l.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:39:00.758472+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
18	m	6F8F57715090DA2632453988D9A1501B	                                	m@m.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:39:11.801147+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
19	n	7B8B965AD4BCA0E41AB51DE7B31363A1	                                	n@n.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:39:22.979953+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
20	o	D95679752134A2D9EB61DBD7B91C4BCC	                                	o@o.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:39:33.402113+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
21	p	83878C91171338902E0FE0FB97A8C47A	                                	p@p.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:39:42.251064+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
22	q	7694F4A66316E53C8CDD9D9954BD611D	                                	q@q.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:40:04.614632+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
23	r	4B43B0AEE35624CD95B910189B3DC231	                                	r@r.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:40:25.895375+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
24	s	03C7C0ACE395D80182DB07AE2C30F034	                                	s@s.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:40:36.897988+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
25	t	E358EFA489F58062F10DD7316B65649E	                                	t@t.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:40:53.285861+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
26	u	7B774EFFE4A349C6DD82AD4F4F21D34C	                                	u@u.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:41:03.586951+08	\N	\N	\N	2	3	::1            	\N	\N	\N	1	\N
37	le	D9180594744F870AEEFB086982E980BB	                                	le@le.com	\N	\N	\N	\N	\N	\N	\N	0	2014-04-05 16:42:24.932706+08	\N	\N	\N	3	3	::1            	\N	\N	\N	1	le
4	admin	69BFC4EF467B367E3515CDCF693E65DB	                                	djs@olexe.cn	\N	\N	100	 	0	0	0	0	2014-03-07 20:55:42.548339+08	\N	\N	 	1	1	222.209.110.12 	 	\N	 	1	管理员大人
9	alele	69BFC4EF467B367E3515CDCF693E65DB	                                	a@a.com	\N	aa。com	\N	\N	\N	\N	\N	0	2014-03-09 18:36:01.701832+08	\N	\N	\N	3	3	::1            	\N	\N	\N	1	alele
16	j	363B122C528F54DF4A0446B6BAB05515	                                	j@j.com	\N	\N	\N	\N	\N	\N	\N	0	2014-03-09 18:38:26.977939+08	\N	\N	\N	3	3	::1            	\N	\N	\N	1	j
\.


--
-- Name: jexus_categories_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY categories
    ADD CONSTRAINT jexus_categories_pkey PRIMARY KEY (cid, pid);


--
-- Name: jexus_comments_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY comments
    ADD CONSTRAINT jexus_comments_pkey PRIMARY KEY (id, fid, uid);


--
-- Name: jexus_favorites_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY favorites
    ADD CONSTRAINT jexus_favorites_pkey PRIMARY KEY (id, uid);


--
-- Name: jexus_forums_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY forums
    ADD CONSTRAINT jexus_forums_pkey PRIMARY KEY (fid, cid, uid);


--
-- Name: jexus_links_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY links
    ADD CONSTRAINT jexus_links_pkey PRIMARY KEY (id);


--
-- Name: jexus_notifications_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY notifications
    ADD CONSTRAINT jexus_notifications_pkey PRIMARY KEY (nid, nuid);


--
-- Name: jexus_page_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY page
    ADD CONSTRAINT jexus_page_pkey PRIMARY KEY (pid);


--
-- Name: jexus_settings_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY settings
    ADD CONSTRAINT jexus_settings_pkey PRIMARY KEY (sid, title, type);


--
-- Name: jexus_tags_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY tags
    ADD CONSTRAINT jexus_tags_pkey PRIMARY KEY (tag_id);


--
-- Name: jexus_user_follow_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY user_follow
    ADD CONSTRAINT jexus_user_follow_pkey PRIMARY KEY (follow_id, uid, follow_uid);


--
-- Name: jexus_users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres; Tablespace: 
--

ALTER TABLE ONLY users
    ADD CONSTRAINT jexus_users_pkey PRIMARY KEY (uid, group_type);


--
-- Name: public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE ALL ON SCHEMA public FROM PUBLIC;
REVOKE ALL ON SCHEMA public FROM postgres;
GRANT ALL ON SCHEMA public TO postgres;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- PostgreSQL database dump complete
--

